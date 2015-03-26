#pragma once

/* Definitions */
#define CHEAT_PROGRAM_COUNT		6

/* Enums */
enum HackNotificationType
{
	DebugHack = 0,
	MemoryHack = 1,
};

/* Enums */
enum CheatNotificationType : BYTE
{
	Executable = 0,
	WindowTitle = 1,
};

/* Structs */
struct CheatProgram
{
	BYTE Id;
	char* FileName;
	char* WindowName;

	CheatProgram(BYTE id, char* fileName, char* windowName)
	{
		Id = id;
		FileName = fileName;
		WindowName = windowName;
	}

	CheatProgram()
	{
	}
};

/* Exports */
extern __declspec(dllexport) char* g_pSerial;

/* Prototypes */
extern void InitSecurity();
extern void SendHackNotification(HackNotificationType type);
