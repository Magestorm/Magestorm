using System;

namespace MageServer
{
    public class ArenaRuleset
    {
        public enum ArenaMode
        {
            Normal,
            TwoTeams,
            FreeForAll,
            CaptureTheFlag,
            Deathmatch,
            ExpEvent,
            Custom,
        }

        [Flags]
        public enum ArenaRule
        {
            None = 0x0,
            NoHinder = 0x1,
            NoTapping = 0x2,
            NoRaiseCall = 0x4,
            NoPoolBiasing = 0x8,
            NoShrineBiasing = 0x10,
            NoTeams = 0x20,
            NoRegen = 0x40,
            NoSolidWalls = 0x80,
            TwoTeams = 0x100,
            FastRegen = 0x200,
            NoFriendlyOther = 0x400,
            GuildRules = 0x800,
            ExpEvent = 0x1000,
            CaptureTheFlag = 0x2000,
            FriendlyFire = 0x4000,
			ShrineProtection = 0x8000,
        }

        public ArenaRule Rules;
        public ArenaMode Mode;

        public String ModeString
        {
            get
            {
                switch (Mode)
                {
                    case ArenaMode.TwoTeams:
                    {
                        return "2T";
                    }
                    case ArenaMode.FreeForAll:
                    {
                        return "FFA";
                    }
                    case ArenaMode.CaptureTheFlag:
                    {
                        return "CTF";
                    }
                    case ArenaMode.Deathmatch:
                    {
                        return "DM";
                    }
                    case ArenaMode.ExpEvent:
                    {
                        return "EXP";
                    }
                    case ArenaMode.Custom:
                    {
                        return "C";
                    }
                }

                return "N";
            }
        }

        public ArenaRuleset(ArenaMode mode)
        {
            Mode = mode;

            switch (mode)
            {
                case ArenaMode.Normal:
                {
                    Rules = ArenaRule.None;
                    break;
                }
                case ArenaMode.TwoTeams:
                {
                    Rules = ArenaRule.TwoTeams;
                    break;
                }
                case ArenaMode.FreeForAll:
                {
                    Rules = ArenaRule.FastRegen | ArenaRule.NoPoolBiasing | ArenaRule.NoShrineBiasing | ArenaRule.NoTeams | ArenaRule.NoRaiseCall | ArenaRule.NoFriendlyOther;
                    break;
                }
                case ArenaMode.CaptureTheFlag:
                {
                    Rules = ArenaRule.CaptureTheFlag;
                    break;
                }
                case ArenaMode.Deathmatch:
                {
                    Rules = ArenaRule.NoTapping | ArenaRule.NoShrineBiasing | ArenaRule.NoRaiseCall;
                    break;
                }
                case ArenaMode.ExpEvent:
                {
                    Rules = ArenaRule.ExpEvent;
                    break;
                }
            }
        }

        public ArenaRuleset(ArenaRule rules)
        {
            Mode = ArenaMode.Custom;
            Rules = rules;
        }

        public static ArenaRule GetRuleFromString(String ruleString)
        {
            switch (ruleString.ToLower())
            {
                case "nohinder":
                case "nohind":
                case "nopara":
				case "noh":
				case "nh":
                {
                    return ArenaRule.NoHinder;
                }
                case "notapping":
                case "notapp":
                case "notap":
				case "not":
				case "nt":
                {
                    return ArenaRule.NoTapping;
                }
                case "noraisecall":
                case "noraise":
                case "nocall":
                case "nocal":
                case "norez":
                case "norai":
				case "nor":
				case "nr":
                {
                    return ArenaRule.NoRaiseCall;
                }
                case "nopoolbiasing":
                case "nopoolbias":
				case "nopool":
				case "nop":
				case "np":
                {
                    return ArenaRule.NoPoolBiasing;
                }
                case "noshrinebiasing":
                case "noshrinebias":
				case "noshrine":
				case "nos":
				case "ns":
                {
                    return ArenaRule.NoShrineBiasing;
                }
                case "noteams":
                case "noteam":
				case "notm":
				case "ntm":
                {
                    return ArenaRule.NoTeams;
                }
                case "noregen":
				case "norgn":
				case "nrgn":
                {
                    return ArenaRule.NoRegen;
                }
                case "nosolidwalls":
				case "nosolids":
				case "nosolid":
				case "nosw":
                case "nsw":
                {
                    return ArenaRule.NoSolidWalls;
                }
                case "2teams":
                case "2team":
                case "2t":
                {
                    return ArenaRule.TwoTeams;
                }
                case "fastregen":
				case "fr":
                {
                    return ArenaRule.FastRegen;
                }
                case "nohealother":
                case "noho":
                {
                    return ArenaRule.NoFriendlyOther;
                }
                case "guildrules":
                case "guildmatch":
                case "guild":
                case "gr":
                case "gm":
                {
                    return ArenaRule.GuildRules;
                }
                case "expevent":
                case "exp":
                case "event":
                case "ev":
                {
                    return ArenaRule.ExpEvent;
                }
                case "capturetheflag":
                case "capture":
                case "flag":
                case "ctf":
				case "cto":
                {
                    return ArenaRule.CaptureTheFlag;
                }
                case "friendlyfire":
                case "ff":
                {
                    return ArenaRule.FriendlyFire;
                }
				case "shrineprotection":
				case "shrineprot":
				case "sp":
				case "protection":
				case "prot":
	            {
		            return ArenaRule.ShrineProtection;
	            }
            }

            return ArenaRule.None;
        }

        public static ArenaMode GetModeFromString(String modeString)
        {
            switch (modeString.ToLower())
            {
                case "normal":
				case "norm":
                case "n":
                {
                    return ArenaMode.Normal;
                }
                case "twoteams":
                case "twoteam":
                case "2teams":
                case "2team":
                case "2t":
                {
                    return ArenaMode.TwoTeams;
                }
                case "freeforall":
                case "free":
                case "ffa":
                {
                    return ArenaMode.FreeForAll;
                }
                case "deathmatch":
                case "dm":
                {
                    return ArenaMode.Deathmatch;
                }
                case "custom":
                case "c":
                {
                    return ArenaMode.Custom;
                }
                case "expevent":
                case "exp":
                case "event":
                case "ev":
                {
                    return ArenaMode.ExpEvent;
                }
                case "capturetheflag":
                case "capture":
                case "flag":
                case "ctf":
                {
                    return ArenaMode.CaptureTheFlag;
                }
            }

            return ArenaMode.Normal;
        }
    }
}
