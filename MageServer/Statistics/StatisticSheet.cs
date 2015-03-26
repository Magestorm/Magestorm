using System;

namespace MageServer.Statistics
{
    public class StatisticSheet
    {
        public readonly Int32 CharacterId;
		public readonly Int32 Hidden;

        public Int64 Kills;
        public Int64 Deaths;
        public Int64 Raises;
        public Int64 DamageDone;
        public Int64 DamageTaken;
        public Int64 HealingDone;
        public Int64 HealingTaken;
        public Int64 Wins;
        public Int64 Losses;

        public StatisticSheet(Player player, Character character)
        {
            CharacterId = character.CharacterId;

            if (player.Admin == AdminLevel.Tester || character.OpLevel > 0)
            {
                Hidden = 1;
            }
            else
            {
                Hidden = 0;
            }
        }

        public void Save()
        {
            if (CharacterId <= 0) return;

			MySQL.CharacterStatistics.OverallUpdate(this);
	        MySQL.CharacterStatistics.WeeklyUpdate(this);

            Kills = 0;
            Deaths = 0;
            Raises = 0;
            DamageDone = 0;
            DamageTaken = 0;
            HealingDone = 0;
            HealingTaken = 0;
            Wins = 0;
            Losses = 0;
        }
    }
}
