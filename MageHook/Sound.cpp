#include <windows.h>
#include <stdio.h>
#include "Sound.h"
#include "Bass.h"
#include "Generic.h"
#include "Game.h"

Sound SoundHook = Sound();

Sound::Sound()
{
	IsBassEnabled = false;
	StreamHandle = NULL;
}

void Sound::Initialize()
{
	if (BASS_Init(-1, 44100, 0, 0, 0))
	{
		IsBassEnabled = true;
	}
	else
	{
		IsBassEnabled = false;
	}

	StreamHandle = NULL;
}

DWORD WINAPI PlayWebMusicThread(LPVOID lpParam)
{
	char* songName = (char*)lpParam;
	char buffer[256];

	sprintf_s(buffer, 256, "http://music.magestorm.net/%s.mp3", songName);

	if (_strcmpi(songName, "stop") == 0)
	{
		if (SoundHook.StreamHandle != NULL)
		{
			BASS_ChannelStop(SoundHook.StreamHandle);
			GameHook.PrintChatMessage("Song stopped.");
		}
	}
	else if (_strcmpi(songName, "play") == 0)
	{
		if (SoundHook.StreamHandle != NULL)
		{
			BASS_ChannelPlay(SoundHook.StreamHandle, false);
			GameHook.PrintChatMessage("Resuming song...");
		}
	}
	else
	{
		if (SoundHook.StreamHandle != NULL)
		{
			BASS_ChannelStop(SoundHook.StreamHandle);
			BASS_StreamFree(SoundHook.StreamHandle);
			SoundHook.StreamHandle = NULL;
		}

		SoundHook.StreamHandle = BASS_StreamCreateURL(buffer, 0, BASS_STREAM_BLOCK, 0, 0);

		if (SoundHook.StreamHandle)
		{
			BASS_ChannelPlay(SoundHook.StreamHandle, TRUE);
			GameHook.PrintChatMessageEx("Playing %s...", songName);
		}
		else
		{
			GameHook.PrintChatMessageEx("There was an error playing a song.  (Err: %i, File: %s.mp3)", BASS_ErrorGetCode(), songName);
		}
	}

	delete[] songName;

	return 0;
}

__declspec(dllexport) void _stdcall PlayWebMusic(char* songName)
{
	if (SoundHook.IsBassEnabled)
	{
		CreateThread(NULL, 0, PlayWebMusicThread, CopyBuffer(songName, 128), 0, NULL);
	}
}
