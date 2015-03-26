#include <windows.h>
#include "detours.h"

/* Prototypes */
void (__stdcall *CalculateThinsHook)();
void (__stdcall *CalculateObjectsHook)();
void (__stdcall *CalculateNearProjectilesHook)();
void (__stdcall *CalculateFarProjectilesHook)();
void (__stdcall *UnknownExceptionHook)();

void CalculateThins();
void CalculateObjects();
void CalculateNearProjectiles();
void CalculateFarProjectiles();
void UnknownException();

void DetourExceptionHooks()
{
	DetourRestoreAfterWith();
	DetourTransactionBegin();
	DetourUpdateThread(GetCurrentThread());

	CalculateThinsHook = (void(__stdcall*)())0x49D546;
	CalculateObjectsHook = (void(__stdcall*)())0x49D3EB;
	CalculateNearProjectilesHook = (void(__stdcall*)())0x49D79A;
	CalculateFarProjectilesHook = (void(__stdcall*)())0x49D83F;
	UnknownExceptionHook = (void(__stdcall*)())0x49D1F2;

	DetourAttach(&(PVOID&)CalculateThinsHook, CalculateThins);
	DetourAttach(&(PVOID&)CalculateObjectsHook, CalculateObjects);
	DetourAttach(&(PVOID&)CalculateNearProjectilesHook, CalculateNearProjectiles);
	DetourAttach(&(PVOID&)CalculateFarProjectilesHook, CalculateFarProjectiles);
	DetourAttach(&(PVOID&)UnknownExceptionHook, UnknownException);

	DetourTransactionCommit();
}

void CalculateThins()
{
	__try
	{
		CalculateThinsHook();
	}
	__except (EXCEPTION_EXECUTE_HANDLER)
	{
	}
}

void CalculateObjects()
{
	__try
	{
		CalculateObjectsHook();
	}
	__except (EXCEPTION_EXECUTE_HANDLER)
	{
	}
}

void CalculateNearProjectiles()
{
	__try
	{
		CalculateNearProjectilesHook();
	}
	__except (EXCEPTION_EXECUTE_HANDLER)
	{
	}
}

void CalculateFarProjectiles()
{
	__try
	{
		CalculateFarProjectilesHook();
	}
	__except (EXCEPTION_EXECUTE_HANDLER)
	{
	}
}

void UnknownException()
{
	__try
	{
		UnknownExceptionHook();
	}
	__except (EXCEPTION_EXECUTE_HANDLER)
	{
	}
}