#include <windows.h>
#include <stdio.h>
#include "Game.h"
#include "Generic.h"
#include "Input.h"

/* Prototypes */
LRESULT CALLBACK ProcessKeyboard(int nCode, WPARAM wp, LPARAM lp);
LRESULT CALLBACK ProcessKeyboardLL(int nCode, WPARAM wp, LPARAM lp);
LRESULT CALLBACK ProcessMouse(int nCode, WPARAM wp, LPARAM lp);

Input InputHook = Input();

Input::Input()
{
	MouseVerticalMode = Normal;
	SpellKeys = new DWORD[12];

	KeyboardHook = NULL;
	LowLevelKeyboardHook = NULL;
	MouseHook = NULL;
}

void Input::Initialize()
{	
	KeyboardHook = SetWindowsHookEx(WH_KEYBOARD, ProcessKeyboard, GameHook.HookInstance, GetCurrentThreadId());
	LowLevelKeyboardHook = SetWindowsHookEx(WH_KEYBOARD_LL, ProcessKeyboardLL, GameHook.HookInstance, 0);
	MouseHook = SetWindowsHookEx(WH_MOUSE, ProcessMouse, GameHook.HookInstance, GetCurrentThreadId());

	MouseVerticalMode = (VerticalMode)GetPrivateProfileInt("main", "fullmousemode", 1, "./User.dat");

	for (int x = 0; x < 12; x++)
	{
		char buffer[256];
		sprintf_s(buffer, 256, "spellkey%d", x);

		DWORD keyNum = GetPrivateProfileInt("spellkeys", buffer, 0, "./User.dat");

		if (keyNum != 0)
		{
			SpellKeys[x] = keyNum;
		}
		else
		{
			switch (x)
			{
				case 9:
				{
					SpellKeys[x] = 0x30;
					break;
				}
				case 10:
				{
					SpellKeys[x] = 0xBD;
					break;
				}
				case 11:
				{
					SpellKeys[x] = 0xBB;
					break;
				}
				default:
				{
					SpellKeys[x] = x + 0x31;
					break;
				}
			}
		}
	}
}

LRESULT CALLBACK ProcessKeyboard(int nCode, WPARAM wp, LPARAM lp)
{
	if (IsKeyDown(lp) && !*GameHook.InChatMode)
	{
		switch (*GameHook.CurrentGameWindow)
		{
			case Arena:
			{
				for (int x = 0; x < 12 ; x++)
				{
					if (wp == InputHook.SpellKeys[x])
					{
						GameHook.SetCurrentSpellSlot(x);
						return true;
					}
				}

				switch (wp)
				{
					case VK_F12:
					{
						switch (InputHook.MouseVerticalMode)
						{
							case Disabled:
							{
								InputHook.MouseVerticalMode = Normal;
								GameHook.PrintChatMessage("Vertical Mouse Look is now enabled.");
								break;
							}
							case Normal:
							{
								InputHook.MouseVerticalMode = Inverted;
								GameHook.PrintChatMessage("Vertical Mouse Look is now inverted.");
								break;
							}
							case Inverted:
							default:
							{
								InputHook.MouseVerticalMode = Disabled;
								GameHook.PrintChatMessage("Vertical Mouse Look is now disabled.");
								break;
							}
						}

						return true;
					}
				}
				break;
			}
		}
	}

	return CallNextHookEx(InputHook.KeyboardHook, nCode, wp, lp);
}

LRESULT CALLBACK ProcessKeyboardLL(int nCode, WPARAM wp, LPARAM lp)
{
	KBDLLHOOKSTRUCT *info = (KBDLLHOOKSTRUCT*) lp;

	if (nCode == HC_ACTION)
	{
		switch (*GameHook.CurrentGameWindow)
		{
			case Arena:
			{
				switch (info->vkCode)
				{
					case VK_LWIN:
					case VK_RWIN:
					{
						return true;
					}
				}
				break;
			}
		}

	}
	return CallNextHookEx(InputHook.LowLevelKeyboardHook, nCode, wp, lp);
}

LRESULT CALLBACK ProcessMouse(int nCode, WPARAM wp, LPARAM lp)
{
	if (nCode == HC_ACTION)
	{
		MOUSEHOOKSTRUCTEX *info = (MOUSEHOOKSTRUCTEX*) lp;
		switch (wp)
		{
			case WM_MOUSEMOVE:
			{
				switch (*GameHook.CurrentGameWindow)
				{
					case Arena:
					{
						if (!GameHook.ICamera) break;

						switch (InputHook.MouseVerticalMode)
						{
							case Normal:
							{
								if (info->pt.y < 239)
								{
									int delta = ((239 - info->pt.y) / 16) + 1;
									if (GameHook.ICamera->Info->Tilt - delta > GameHook.ICamera->Info->MaxTiltDown)
									{
										GameHook.ICamera->Info->Tilt -= delta;
									}
									else
									{
										GameHook.ICamera->Info->Tilt = GameHook.ICamera->Info->MaxTiltDown;
									}
								}
								else if (info->pt.y > 241)
								{
									int delta = ((info->pt.y - 241) / 16) + 1;
									if (GameHook.ICamera->Info->Tilt + delta < GameHook.ICamera->Info->MaxTiltUp)
									{
										GameHook.ICamera->Info->Tilt += delta;
									}
									else
									{
										GameHook.ICamera->Info->Tilt = GameHook.ICamera->Info->MaxTiltUp;
									}
								}
								break;
							}
							case Inverted:
							{
								if (info->pt.y < 239)
								{
									int delta = ((239 - info->pt.y) / 16) + 1;
									if (GameHook.ICamera->Info->Tilt + delta < GameHook.ICamera->Info->MaxTiltUp)
									{
										GameHook.ICamera->Info->Tilt += delta;
									}
									else
									{
										GameHook.ICamera->Info->Tilt = GameHook.ICamera->Info->MaxTiltUp;
									}
								}
								else if (info->pt.y > 241)
								{
									int delta = ((info->pt.y - 241) / 16) + 1;
									if (GameHook.ICamera->Info->Tilt - delta > GameHook.ICamera->Info->MaxTiltDown)
									{
										GameHook.ICamera->Info->Tilt -= delta;
									}
									else
									{
										GameHook.ICamera->Info->Tilt = GameHook.ICamera->Info->MaxTiltDown;
									}
								}
								break;
							}
						}

						break;
					}
				}

				break;
			}
			case WM_MOUSEWHEEL:
			{
				switch (*GameHook.CurrentGameWindow)
				{
					case Arena:
					{
						short delta = GET_WHEEL_DELTA_WPARAM(info->mouseData);
						bool IsWheelUp = delta >= 0 ? true : false;

						if (IsWheelUp)
						{
							
						}
						else
						{

						}

						break;
					}
				}

				break;
			}

			case WM_LBUTTONUP:
			{
				break;
			}

			case WM_LBUTTONDOWN:
			{
				break;
			}

			case WM_RBUTTONUP:
			{
				break;
			}

			case WM_RBUTTONDOWN:
			{
				break;
			}

			case WM_MBUTTONUP:
			{
				break;
			}

			case WM_MBUTTONDOWN:
			{
				break;
			}
		}
	}

	return CallNextHookEx(InputHook.MouseHook, nCode, wp, lp);
}