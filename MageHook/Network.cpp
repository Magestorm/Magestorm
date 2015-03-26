#include <windows.h>
#include "Network.h"

void(__stdcall *SendPacketHook)(int, int, int) = (void(__stdcall*)(int, int, int))0x433430;
void(__stdcall *DisconnectHook)() = (void(__stdcall*)())0x42B1C0;

Network NetworkHook = Network();

Network::Network()
{
}

void Network::Send(int dataSize, int function, char* buffer)
{
	__asm
	{
		push	0
		push	0
		push	dataSize
		mov		edx, function
		mov		ecx, buffer
		call	SendPacketHook
	}
}

void Network::Disconnect()
{
	__asm
	{
		call	DisconnectHook
	}
}