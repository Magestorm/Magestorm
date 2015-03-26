#pragma once
#include <windows.h>

/* Enums */

enum CharDataIndex
{
	Level,
	Experience,
	Unknown1,
	Unknown2,
	Unknown3,
	Agility,
	Constitution,
	Memory,
	Reasoning,
	Discipline,
	Empathy,
	Intuition,
	Presence,
	Quickness,
	Strength,
	Agility2,
	Constitution2,
	Memory2,
	Reasoning2,
	Discipline2,
	Empathy2,
	Intuition2,
	Presence2,
	Quickness2,
	Strength2,
	SpellKey1,
	SpellKey2,
	SpellKey3,
	SpellKey4,
	SpellKey5,
	SpellKey6,
	SpellKey7,
	SpellKey8,
	SpellKey9,
	SpellKey10,
	SpellKey11,
	SpellKey12,
};
enum GameWindow
{
	Exited = -4,
	None = -1,
	Study = 0,
	Arena = 1,
	Tavern = 2,
	Message = 3,
	Login = 4,
	MatchScores = 5,
	KeyboardConfig = 6,
	Character = 7,
	Credits = 8,
	HighScores = 13,
};

/* Structs */
struct CameraInfo
{
	int u1;
	int u2;
	int u3;
	int u4;
	int u5;
	int u6;
	int u7;
	int u8;
	int u9;
	int u10;
	int u11;
	int ZPositionStanding;
	int ZPositionCrouching;
	int u14;
	int Tilt;
	int MaxTiltDown;
	int MaxTiltUp;
	int u18;
	int u19;
};

struct Camera
{
	CameraInfo* Info;
};

/* Classes */
class Game
{
private:

public:
	HINSTANCE HookInstance;

	Camera* ICamera;
	GameWindow* CurrentGameWindow;
	bool* InChatMode;

	Game::Game();

	~Game()
	{
	}

	void Game::Initialize();
	int Game::GetCharDataByIndex(int index);
	void Game::SetCharDataByIndex(int index, int value);
	void Game::SetCurrentSpellSlot(int slotNumber);
	int Game::PrintChatMessage(char* text);
	void Game::PrintChatMessageEx(char* text, ...);
};

/* Globals */
extern Game GameHook;