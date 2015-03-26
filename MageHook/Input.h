#pragma once
#include <windows.h>

/* Enums */
enum VerticalMode
{
	Disabled = 0,
	Normal = 1,
	Inverted = 2,
};

/* Definitions */
#define IsKeyDown(lp) ((0x80000000 & lp) == 0)

/* Classes */
class Input
{
private:

public:
	VerticalMode MouseVerticalMode;
	DWORD* SpellKeys;

	HHOOK KeyboardHook;
	HHOOK LowLevelKeyboardHook;
	HHOOK MouseHook;

	Input::Input();

	~Input()
	{
	}

	void Input::Initialize();
};

/* Globals */
extern Input InputHook;