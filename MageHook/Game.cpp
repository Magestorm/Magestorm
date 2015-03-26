#include <windows.h>
#include <stdio.h>
#include "Game.h"

int(__stdcall *PrintChatMessageHook)(int unk1, int unk2) = (int(__stdcall*)(int, int))0x42C750;
void(__stdcall *SetCurrentSpellSlotHook)() = (void(__stdcall*)())0x416DB0;
int(__stdcall *GetCharDataByIndexHook)(int index) = (int(__stdcall*)(int))0x468180;
void(__stdcall *SetCharDataByIndexHook)(int value, int unk, int index, int function) = (void(__stdcall*)(int , int, int, int))0x4680C0;

Game GameHook = Game();

Game::Game()
{
	ICamera = (Camera*)0x6AA008;
	CurrentGameWindow = (GameWindow*)0x6F5E98;
	InChatMode = (bool*)0x613A34;
}

void Game::Initialize()
{
}

int Game::GetCharDataByIndex(int index)
{
	return GetCharDataByIndexHook(index);
}

void Game::SetCharDataByIndex(int index, int value)
{
	SetCharDataByIndexHook(value, 0, index, 2);
}

void Game::SetCurrentSpellSlot(int slot)
{
	if (*CurrentGameWindow != Arena || slot < 0 || slot > 11) return;

	__asm
	{
		push	edx
		mov		edx, slot
		push	ecx
		mov		ecx, dword ptr[0x6AA008]
		call	SetCurrentSpellSlotHook
		pop		ecx
		pop		edx
	}
}

int Game::PrintChatMessage(char* text)
{
	__asm
	{
		mov     edx, dword ptr[text]
		mov		ecx, -2
		push	0
		push    0
		call	PrintChatMessageHook
	}
}

void Game::PrintChatMessageEx(char* text, ...)
{
	char buffer[256];
	va_list args;
	va_start(args, text);
	vsprintf_s(buffer, text, args);
	PrintChatMessage(buffer);
	va_end(args);
}