#include <windows.h>
#include <string.h>
#include <winioctl.h>
#include <iostream>
#include "Generic.h"
#include "HDDSerial.h"

BYTE IdOutCmd[sizeof (SENDCMDOUTPARAMS)+IDENTIFY_BUFFER_SIZE - 1];

BOOL DoIdentify (HANDLE hPhysicalDriveIOCTL, PSENDCMDINPARAMS pSCIP, PSENDCMDOUTPARAMS pSCOP, BYTE bIDCmd, BYTE bDriveNum, PDWORD lpcbBytesReturned)
{
	pSCIP->cBufferSize = IDENTIFY_BUFFER_SIZE;
	pSCIP->irDriveRegs.bFeaturesReg = 0;
	pSCIP->irDriveRegs.bSectorCountReg = 1;
	pSCIP->irDriveRegs.bCylLowReg = 0;
	pSCIP->irDriveRegs.bCylHighReg = 0;
	pSCIP->irDriveRegs.bDriveHeadReg = 0xA0 | ((bDriveNum & 1) << 4);
	pSCIP->irDriveRegs.bCommandReg = bIDCmd;
	pSCIP->bDriveNumber = bDriveNum;

	return (DeviceIoControl (hPhysicalDriveIOCTL, DFP_RECEIVE_DRIVE_DATA, (LPVOID) pSCIP, sizeof(SENDCMDINPARAMS) - 1, (LPVOID) pSCOP, sizeof(SENDCMDOUTPARAMS) + IDENTIFY_BUFFER_SIZE - 1, lpcbBytesReturned, NULL));
}

char* ConvertToString (DWORD diskdata[256], int firstIndex, int lastIndex, char* buf)
{
	int index = 0;
	int position = 0;

	for (index = firstIndex; index <= lastIndex; index++)
	{
		buf [position++] = (char) (diskdata [index] / 256);
		buf [position++] = (char) (diskdata [index] % 256);
	}

	buf[position] = '\0';

	for (index = position - 1; index > 0 && isspace(buf [index]); index--) buf [index] = '\0';

	return buf;
}

void CleanSerialString(const char* str, int pos, char* buf)
{
	int strPos = pos;
	int bufPos = 0;

	while(str[strPos] != 0)
	{
		char c = str[strPos++];

		if (isalnum(c) || c == '_' || c == '-')
		{
			buf[bufPos++] = c;
		}
	}

	buf[bufPos] = '\0';
}

bool ReadPhysicalDriveInNTWithAdminRights(char* &serial)
{
	bool success = false;

	for (int drive = 0; drive < 16 && !success; drive++)
	{
		HANDLE hPhysicalDriveIOCTL = 0;
		char driveName [256];

		sprintf_s(driveName, "\\\\.\\PhysicalDrive%d", drive);

		hPhysicalDriveIOCTL = CreateFile (driveName, GENERIC_READ | GENERIC_WRITE, FILE_SHARE_READ | FILE_SHARE_WRITE , NULL, OPEN_EXISTING, 0, NULL);

		if (hPhysicalDriveIOCTL != INVALID_HANDLE_VALUE)
		{
			GETVERSIONOUTPARAMS VersionParams;
			DWORD cbBytesReturned = 0;

			memset ((void*) &VersionParams, 0, sizeof(VersionParams));

			if (DeviceIoControl (hPhysicalDriveIOCTL, DFP_GET_VERSION, NULL, 0, &VersionParams, sizeof(VersionParams), &cbBytesReturned, NULL))
			{    
				if (VersionParams.bIDEDeviceMap > 0)
				{
					BYTE bIDCmd = 0;
					SENDCMDINPARAMS  scip;

					bIDCmd = (VersionParams.bIDEDeviceMap >> drive & 0x10) ? IDE_ATAPI_IDENTIFY : IDE_ATA_IDENTIFY;

					memset (&scip, 0, sizeof(scip));
					memset (IdOutCmd, 0, sizeof(IdOutCmd));

					if (DoIdentify (hPhysicalDriveIOCTL, &scip, (PSENDCMDOUTPARAMS)&IdOutCmd, (BYTE) bIDCmd, (BYTE) drive, &cbBytesReturned))
					{
						DWORD diskdata [256];
						int ijk = 0;
						USHORT *pIdSector = (USHORT *) ((PSENDCMDOUTPARAMS) IdOutCmd) -> bBuffer;

						for (ijk = 0; ijk < 256; ijk++) diskdata [ijk] = pIdSector [ijk];

						char* buf = CreateBuffer(1024);

						ConvertToString(diskdata, 10, 19, buf);
						CleanSerialString(buf, 0, serial);

						if (strlen(serial) >= 4 && !StartsWith("ARRAY", serial))
						{
							success = true;
						}			
					}
				}
			}

			CloseHandle (hPhysicalDriveIOCTL);
		}
	}

	return success;
}

bool ReadIdeDriveAsScsiDriveInNT(char* &serial)
{
	bool success = false;
	int controller = 0;

	for (controller = 0; controller < 16 && !success; controller++)
	{
		HANDLE hScsiDriveIOCTL = 0;
		char driveName [256];

		sprintf_s(driveName, "\\\\.\\Scsi%d:", controller);

		hScsiDriveIOCTL = CreateFile (driveName, GENERIC_READ | GENERIC_WRITE, FILE_SHARE_READ | FILE_SHARE_WRITE, NULL, OPEN_EXISTING, 0, NULL);

		if (hScsiDriveIOCTL != INVALID_HANDLE_VALUE)
		{
			int drive = 0;

			char buffer [sizeof (SRB_IO_CONTROL) + SENDIDLENGTH];
			SRB_IO_CONTROL *p = (SRB_IO_CONTROL *) buffer;
			SENDCMDINPARAMS *pin = (SENDCMDINPARAMS *) (buffer + sizeof (SRB_IO_CONTROL));
			DWORD dummy;

			memset (buffer, 0, sizeof (buffer));

			p -> HeaderLength = sizeof (SRB_IO_CONTROL);
			p -> Timeout = 10000;
			p -> Length = SENDIDLENGTH;
			p -> ControlCode = IOCTL_SCSI_MINIPORT_IDENTIFY;

			strncpy_s((char*)p->Signature, 9, "SCSIDISK", 8);

			pin -> irDriveRegs.bCommandReg = IDE_ATA_IDENTIFY;
			pin -> bDriveNumber = drive;

			if (DeviceIoControl (hScsiDriveIOCTL, IOCTL_SCSI_MINIPORT, buffer, sizeof (SRB_IO_CONTROL) + sizeof (SENDCMDINPARAMS) - 1, buffer, sizeof (SRB_IO_CONTROL) + SENDIDLENGTH, &dummy, NULL))
			{
				SENDCMDOUTPARAMS *pOut = (SENDCMDOUTPARAMS *) (buffer + sizeof (SRB_IO_CONTROL));
				IDSECTOR *pId = (IDSECTOR *) (pOut -> bBuffer);

				if (pId->sModelNumber[0])
				{
					DWORD diskdata [256];
					int ijk = 0;
					USHORT *pIdSector = (USHORT *) pId;

					for (ijk = 0; ijk < 256; ijk++) diskdata [ijk] = pIdSector [ijk];

					char* buf = CreateBuffer(1024);
					ConvertToString(diskdata, 10, 19, buf);
					CleanSerialString(buf, 0, serial);

					if (strlen(serial) >= 4 && !StartsWith("ARRAY", serial))
					{
						success = true;
					}			
				}
			}

			CloseHandle (hScsiDriveIOCTL);
		}
	}

	return success;
}

bool ReadPhysicalDriveInNTWithZeroRights(char* &serial)
{
	bool success = false;
	int drive = 0;

	for (drive = 0; drive < 16 && !success; drive++)
	{
		HANDLE hPhysicalDriveIOCTL = 0;
		char driveName [256];

		sprintf_s(driveName, "\\\\.\\PhysicalDrive%d", drive);

		hPhysicalDriveIOCTL = CreateFile (driveName, 0, FILE_SHARE_READ | FILE_SHARE_WRITE, NULL, OPEN_EXISTING, 0, NULL);

		if (hPhysicalDriveIOCTL != INVALID_HANDLE_VALUE)
		{
			STORAGE_PROPERTY_QUERY query;
			DWORD cbBytesReturned = 0;
			char buffer [10000];

			memset ((void *) & query, 0, sizeof (query));
			query.PropertyId = StorageDeviceProperty;
			query.QueryType = PropertyStandardQuery;

			memset (buffer, 0, sizeof (buffer));

			if (DeviceIoControl(hPhysicalDriveIOCTL, IOCTL_STORAGE_QUERY_PROPERTY, &query, sizeof (query), &buffer, sizeof (buffer), &cbBytesReturned, NULL))
			{         
				STORAGE_DEVICE_DESCRIPTOR * descrip = (STORAGE_DEVICE_DESCRIPTOR *) & buffer;

				CleanSerialString(buffer, descrip->SerialNumberOffset, serial);

				if (strlen(serial) >= 4 && !StartsWith("ARRAY", serial))
				{
					success = true;
				}			
			}

			CloseHandle (hPhysicalDriveIOCTL);
		}
	}

	return success;
}


bool ReadPhysicalDriveInNTUsingSmart(char* &serial)
{
	bool success = false;
	int drive = 0;

	for (drive = 0; drive < 16 && !success; drive++)
	{
		HANDLE hPhysicalDriveIOCTL = 0;

		char driveName [256];

		sprintf_s(driveName, "\\\\.\\PhysicalDrive%d", drive);

		hPhysicalDriveIOCTL = CreateFile (driveName, GENERIC_READ | GENERIC_WRITE, FILE_SHARE_DELETE | FILE_SHARE_READ | FILE_SHARE_WRITE, NULL, OPEN_EXISTING, 0, NULL);

		if (hPhysicalDriveIOCTL != INVALID_HANDLE_VALUE)
		{
			GETVERSIONINPARAMS GetVersionParams;
			DWORD cbBytesReturned = 0;

			memset ((void*) & GetVersionParams, 0, sizeof(GetVersionParams));

			if (DeviceIoControl (hPhysicalDriveIOCTL, SMART_GET_VERSION, NULL, 0, &GetVersionParams, sizeof (GETVERSIONINPARAMS),&cbBytesReturned, NULL))
			{         
				ULONG CommandSize = sizeof(SENDCMDINPARAMS) + IDENTIFY_BUFFER_SIZE;
				PSENDCMDINPARAMS Command = (PSENDCMDINPARAMS) malloc (CommandSize);

				Command -> irDriveRegs.bCommandReg = 0xEC;
				DWORD BytesReturned = 0;

				if (DeviceIoControl (hPhysicalDriveIOCTL, SMART_RCV_DRIVE_DATA, Command, sizeof(SENDCMDINPARAMS),	Command, CommandSize, &BytesReturned, NULL) )
				{
					DWORD diskdata [256];
					USHORT *pIdSector = (USHORT *)(PIDENTIFY_DATA) ((PSENDCMDOUTPARAMS) Command) -> bBuffer;

					for (int ijk = 0; ijk < 256; ijk++) diskdata [ijk] = pIdSector [ijk];

					char* buf = CreateBuffer(1024);
					ConvertToString(diskdata, 10, 19, buf);
					CleanSerialString(buf, 0, serial);

					if (strlen(serial) >= 4 && !StartsWith("ARRAY", serial))
					{
						success = true;
					}					
				}

				free(Command);
			}

			CloseHandle (hPhysicalDriveIOCTL);
		}
	}

	return success;
}