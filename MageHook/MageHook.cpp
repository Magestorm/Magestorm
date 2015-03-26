#include <windows.h>
#include "Security.h"
#include "ExceptionHooks.h"
#include "Game.h"
#include "Input.h"
#include "Sound.h"

/* Functions */
__declspec(dllexport) void _stdcall InstallHook()
{
	InitSecurity();
	DetourExceptionHooks();
	GameHook.Initialize();
	InputHook.Initialize();
	SoundHook.Initialize();
}

bool __stdcall DllMain(HINSTANCE hInstance, DWORD fwdReason, LPVOID lpvReserved)
{
	if (fwdReason == DLL_PROCESS_ATTACH)
	{
		GameHook.HookInstance = hInstance;
		DisableThreadLibraryCalls(GameHook.HookInstance);		
	}
	else if (fwdReason == DLL_PROCESS_DETACH)
	{
	}

	return true;
}