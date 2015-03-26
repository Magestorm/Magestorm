#include <windows.h>
#include <vector>
#include "Generic.h"

char* CreateBuffer(int size)
{
	char* buf = new char[size];
	memset(buf, 0, size);
	return buf;
}

char* CopyBuffer(char* buffer, int len)
{
	char* copyBuffer = CreateBuffer(len);
	strcpy_s (copyBuffer, len, buffer);
	return copyBuffer;
}

bool StartsWith(const char *pre, const char *str)
{
	size_t lenPre = strlen(pre), lenStr = strlen(str);
	return lenStr < lenPre ? false : strncmp(pre, str, lenPre) == 0;
}

WORD GetCRC(BYTE* ptr, int count)
{
	WORD crc;
	BYTE i;

	crc = 0;

	while (--count >= 0)
	{
		crc = crc ^ (WORD)*ptr++ << 8;
		i = 8;

		do
		{
			if (crc & 0x8000)
				crc = crc << 1 ^ 0x1021;
			else
				crc = crc << 1;
		} while(--i);
	}

	return (crc);
}

MemoryStream::MemoryStream()
{
	m_size = 256;
	m_buffer = CreateBuffer(m_size);
	m_position = 0;
}

MemoryStream::MemoryStream(int size)
{
	m_size = size;
	m_buffer = CreateBuffer(m_size);
	m_position = 0;
}

void MemoryStream::WriteByte(BYTE byte)
{
	if (m_position >= m_size) return;

	m_buffer[m_position++] = byte;
}

char* MemoryStream::GetBuffer()
{
	return m_buffer;
}