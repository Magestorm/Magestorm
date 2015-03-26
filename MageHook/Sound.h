#pragma once
#include <windows.h>

/* Exports */
extern __declspec(dllexport) void _stdcall PlayWebMusic(char* songName);

/* Classes */
class Sound
{
private:

public:

	bool IsBassEnabled;
	DWORD StreamHandle;

	Sound::Sound();

	~Sound()
	{
	}

	void Sound::Initialize();
};

/* Globals */
extern Sound SoundHook;