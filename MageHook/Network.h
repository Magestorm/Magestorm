#pragma once

/* Enums */
enum NetworkHookFunctions
{
	HackNotification = 0xE0,
	CheatProgramNotification = 0xE1,
};

/* Classes */
class Network
{
private:

public:
	Network::Network();

	~Network()
	{
	}

	void Send(int dataSize, int function, char* buffer);
	void Disconnect();
};

/* Globals */
extern Network NetworkHook;
