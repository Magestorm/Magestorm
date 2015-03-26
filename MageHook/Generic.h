#pragma once

/* Prototypes */
extern char* CreateBuffer(int size);
extern char* CopyBuffer(char* buffer, int len);
extern bool StartsWith(const char *pre, const char *str);
extern WORD GetCRC(BYTE* ptr, int count);

class MemoryStream
{
private:

	// Variables
	int m_position;
	int m_size;
	char* m_buffer;

public:
	MemoryStream::MemoryStream();
	MemoryStream::MemoryStream(int size);

	~MemoryStream() 
	{
		delete[] m_buffer;
	}

	void WriteByte(BYTE byte);
	char* GetBuffer();
};