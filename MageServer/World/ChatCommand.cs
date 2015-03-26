using System;
using Helper;

namespace MageServer
{
    public class ChatCommand
    {
        public const String CommandChar = "!";

        public readonly ListCollection<String> Arguments;
        public readonly String Command;

        public ChatCommand(String commandText)
        {
            Command = null;
            Arguments = new ListCollection<String>();

            String[] commandData = commandText.Split(' ');

            if (commandData.Length <= 0 || commandData[0].Length < 2 || !commandData[0].StartsWith(CommandChar)) return;

            Command = commandData[0].Remove(0, 1).ToLower();

            if (commandData.Length <= 1) return;

            for (Int32 i = 1; i <= commandData.Length - 1; i++)
            {
                Arguments.Add(commandData[i]);
            }
        }

        public String GetStringFromArgs(Int32 startArgument)
        {
            if (startArgument >= Arguments.Count) return "";

            String argString = "";

            for (Int32 i = startArgument; i < Arguments.Count; i++)
            {
                argString += Arguments[i] + " ";
            }

            return argString.Trim();
        }
    }
}
