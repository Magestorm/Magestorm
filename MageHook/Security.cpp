#include <windows.h>
#include <tlhelp32.h>
#include "Generic.h"
#include "HDDSerial.h"
#include "WinLicense.h"
#include "Network.h"
#include "Security.h"

/* Exports */
__declspec(dllexport) char* g_pSerial = NULL;

/* Exception Filters */
DWORD __forceinline IsInsideVPC_exceptionFilter(LPEXCEPTION_POINTERS ep);

/* Thread Functions */
DWORD WINAPI DebugProtect(LPVOID lpParam);
DWORD WINAPI MemoryProtect(LPVOID lpParam);
DWORD WINAPI CheatScan(LPVOID lpParam);

/* Prototypes */
bool IsInsideVMWare();
bool IsInsideVPC();
void SetHardwareSerial();
void InitSecurity();

/* Locals */
CheatProgram* CheatPrograms = new CheatProgram[CHEAT_PROGRAM_COUNT];

void InitSecurity()
{
	SetHardwareSerial();

	CODEREPLACE_START
	CheatPrograms[0] = CheatProgram(0, "cheatengine", "cheat engine");
	CheatPrograms[1] = CheatProgram(1, "gamehack", "gamehack");
	CheatPrograms[2] = CheatProgram(2, "gamecheater", "gamecheater");
	CheatPrograms[3] = CheatProgram(3, "tsearch", "tsearch");
	CheatPrograms[4] = CheatProgram(4, "ollydbg", "ollydbg");
	CheatPrograms[5] = CheatProgram(5, "wpe pro", "wpe pro");

	CreateThread(NULL, 0, DebugProtect, NULL, 0, NULL);
	CreateThread(NULL, 0, MemoryProtect, NULL, 0, NULL);
	CreateThread(NULL, 0, CheatScan, NULL, 0, NULL);
	CODEREPLACE_END
}

bool IsInsideVMWare()
{
	bool rc = true;

	__try
	{
		__asm
		{
			push edx
			push ecx
			push ebx
			mov eax, 'VMXh'
			mov ebx, 0
			mov ecx, 10
			mov edx, 'VX'
			in eax, dx
			cmp ebx, 'VMXh'
			setz[rc]
			pop ebx
			pop ecx
			pop edx
		}
	}
	__except (EXCEPTION_EXECUTE_HANDLER)
	{
		rc = false;
	}

	return rc;
}

DWORD __forceinline IsInsideVPC_exceptionFilter(LPEXCEPTION_POINTERS ep)
{
	PCONTEXT ctx = ep->ContextRecord;

	ctx->Ebx = -1;
	ctx->Eip += 4;
	return EXCEPTION_CONTINUE_EXECUTION;
}

bool IsInsideVPC()
{
	bool rc = false;

	__try
	{
		__asm
		{
			push ebx
			mov  ebx, 0
			mov  eax, 1
			__emit 0Fh
			__emit 3Fh
			__emit 07h
			__emit 0Bh
			test ebx, ebx
			setz[rc]
			pop ebx
		}
	}
	__except (IsInsideVPC_exceptionFilter(GetExceptionInformation()))
	{
	}

	return rc;
}

void SetHardwareSerial()
{
	CODEREPLACE_START
	char* serial = CreateBuffer(1024);

	if (!ReadPhysicalDriveInNTUsingSmart(serial) && !ReadIdeDriveAsScsiDriveInNT(serial) && !ReadPhysicalDriveInNTWithAdminRights(serial) && !ReadPhysicalDriveInNTWithZeroRights(serial))
	{
		serial = "Not_Found\0";
	}

	CODEREPLACE_END

	if (IsInsideVMWare())
	{
		CODEREPLACE_START
		serial = "VMWare\0";
		CODEREPLACE_END
	}
	else if (IsInsideVPC())
	{
		CODEREPLACE_START
		serial = "VirtualPC\0";
		CODEREPLACE_END
	}

	CODEREPLACE_START
	g_pSerial = serial;
	CODEREPLACE_END
}

void SendHackNotification(HackNotificationType type)
{
	CODEREPLACE_START
	MemoryStream stream = MemoryStream();
	stream.WriteByte((BYTE)type);
	NetworkHook.Send(1, HackNotification, stream.GetBuffer());
	CODEREPLACE_END
}

void SendCheatProgramNotification(BYTE id, CheatNotificationType type)
{
	CODEREPLACE_START
	MemoryStream stream = MemoryStream();
	stream.WriteByte(id);
	stream.WriteByte(type);
	NetworkHook.Send(2, CheatProgramNotification, stream.GetBuffer());
	CODEREPLACE_END
}

DWORD WINAPI DebugProtect(LPVOID lpParam) 
{
	CODEREPLACE_START
	HANDLE hProcessHeap = GetProcessHeap();
	DWORD dDebugFlag = 0;
	dDebugFlag = 0x1AC0000;

	dDebugFlag = dDebugFlag & 0xFFFFFF;
	dDebugFlag += 0x1000;

	dDebugFlag -= 0x100000;
	bool* isDebugExe = (bool*)dDebugFlag;

	if (*isDebugExe) return 0;

	BOOL debuggerPresent = false;

	CODEREPLACE_END

	while (true)
	{
		CODEREPLACE_START
		debuggerPresent = true;
		CODEREPLACE_END

		__try
		{
			__asm
			{
				int 0x2d
				xor eax, eax
				add eax, 2
			}
		}
		__except(EXCEPTION_EXECUTE_HANDLER)
		{
			CODEREPLACE_START
			debuggerPresent  = false;
			CODEREPLACE_END
		}

		__try
		{
			__asm
			{
				__emit 0xF3
				__emit 0x64
				__emit 0xF1
			}
		}
		__except(EXCEPTION_EXECUTE_HANDLER)
		{
			CODEREPLACE_START
			debuggerPresent = false;
			CODEREPLACE_END
		}

		CODEREPLACE_START
		if (debuggerPresent)
		{
			SendHackNotification(DebugHack);
		}

		Sleep(2500);
		CODEREPLACE_END
	}

	return 0;
}

DWORD WINAPI MemoryProtect(LPVOID lpParam) 
{
	CODEREPLACE_START
	const DWORD dwBaseAddress = 0x401000;
	const DWORD dwBaseSize = 0x7E0AF;

	HANDLE hProcessHeap = GetProcessHeap();
	
	BYTE* pOriginalMemory = (BYTE*)HeapAlloc(hProcessHeap, NULL, dwBaseSize);	
	BYTE* pCurrentMemory = (BYTE*)HeapAlloc(hProcessHeap, NULL, dwBaseSize);	

	ZeroMemory(pCurrentMemory, dwBaseSize);

	memcpy(pOriginalMemory, (LPVOID)dwBaseAddress, dwBaseSize);

	WORD wOriginalChecksum = GetCRC(pOriginalMemory, dwBaseSize);

	if (wOriginalChecksum == 0)
	{
		SendHackNotification(MemoryHack);
	}

	DWORD dDebugFlag = 0;
	dDebugFlag = 0x1AC0000;

	dDebugFlag = dDebugFlag & 0xFFFFFF;
	dDebugFlag += 0x1000;

	dDebugFlag -= 0x100000;
	bool* isDebugExe = (bool*)dDebugFlag;

	if (*isDebugExe) return 0;
	CODEREPLACE_END

	while (true)
	{
		CODEREPLACE_START
		ZeroMemory(pCurrentMemory, dwBaseSize);

		memcpy(pCurrentMemory, (LPVOID)dwBaseAddress, dwBaseSize);

		if (wOriginalChecksum != GetCRC(pCurrentMemory, dwBaseSize))
		{
			SendHackNotification(MemoryHack);
		}

		Sleep(2500);
		CODEREPLACE_END
	}
	
	return 0;
}

BOOL CALLBACK EnumThread(HWND hwnd, LPARAM lParam)
{
	CODEREPLACE_START
	CheatProgram* cheatProgram = (CheatProgram*)lParam;
	TCHAR buffer[50];	

	GetWindowText(hwnd, buffer, 50);

	_strlwr_s(buffer);
	if (strlen(cheatProgram->WindowName) > 0)
	{
		if (strstr(buffer, cheatProgram->WindowName) > 0)
		{
			SendCheatProgramNotification(cheatProgram->Id, WindowTitle);
			return FALSE;
		}
	}
	CODEREPLACE_END
	return TRUE;
}

DWORD WINAPI CheatScan(LPVOID lpParam)
{
	CODEREPLACE_START
	DWORD dDebugFlag = 0;
	dDebugFlag = 0x1AC0000;

	dDebugFlag = dDebugFlag & 0xFFFFFF;
	dDebugFlag += 0x1000;

	dDebugFlag -= 0x100000;
	bool* isDebugExe = (bool*)dDebugFlag;

	if (*isDebugExe) return 0;
	CODEREPLACE_END

	while (true)
	{
		CODEREPLACE_START
		PROCESSENTRY32 processEntry;
		HANDLE hProcessSnapshot = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);

		if (hProcessSnapshot == INVALID_HANDLE_VALUE)
		{
			continue;
		}

		processEntry.dwSize = sizeof(PROCESSENTRY32);

		if (!Process32First(hProcessSnapshot, &processEntry))
		{
			CloseHandle(hProcessSnapshot);
			continue;
		}

		do
		{
			_strlwr_s(processEntry.szExeFile);

			for (int x = 0; x < CHEAT_PROGRAM_COUNT; x++)
			{
				
				if (strstr(processEntry.szExeFile, CheatPrograms[x].FileName))
				{
					SendCheatProgramNotification(CheatPrograms[x].Id, Executable);					
				}
			}

		} while (Process32Next(hProcessSnapshot, &processEntry));
		
		for (int x = 0; x < CHEAT_PROGRAM_COUNT; x++)
		{
			EnumWindows(EnumThread, (LPARAM)&CheatPrograms[x]);
		}

		CloseHandle(hProcessSnapshot);

		Sleep(2000);
		CODEREPLACE_END
	}

	return TRUE;
}