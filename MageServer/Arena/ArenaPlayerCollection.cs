using System;
using System.Linq;
using Helper;

namespace MageServer
{
	public class ArenaPlayerCollection : ListCollection<ArenaPlayer>
	{
		public ArenaPlayerCollection()
		{
			_highestPlayerCount = 0;
		}

		public Byte GetAvailablePlayerId()
        {
            for (Byte i = 1; i <= 254; i++)
            {
                if (FindById(i) == null) return i;
            }
            return 0;
        }

        public ArenaPlayer FindById(Byte playerId)
        {
            return this.FirstOrDefault(arenaPlayer => playerId == arenaPlayer.ArenaPlayerId);
        }
        public ArenaPlayer FindByCharacterId(Int32 characterId)
        {
            return this.FirstOrDefault(arenaPlayer => characterId == arenaPlayer.ActiveCharacter.CharacterId);
        }
        public ArenaPlayer FindByWorldId(Int16 playerId)
        {
            return this.FirstOrDefault(arenaPlayer => playerId == arenaPlayer.WorldPlayer.PlayerId);
        }
        public ArenaPlayer FindByAccountId(Int32 accountId)
        {
            return this.FirstOrDefault(arenaPlayer => accountId == arenaPlayer.WorldPlayer.AccountId);
        }
        public ArenaPlayer FindByUserName(String username)
        {
            return this.FirstOrDefault(arenaPlayer => username == arenaPlayer.WorldPlayer.Username);
        }
        public ArenaPlayer FindByCharacterName(String name)
        {
            return this.FirstOrDefault(p => p.ActiveCharacter != null && name.ToLower() == p.ActiveCharacter.Name.ToLower());
        }
        public ListCollection<ArenaPlayer> FindArenaPlayers(String token)
        {
            token = token.ToLower();

            ListCollection<ArenaPlayer> playerList = new ListCollection<ArenaPlayer>();

            for (Int32 i = Count - 1; i >= 0; i--)
            {
                ArenaPlayer arenaPlayer = this[i];
                if (arenaPlayer == null) continue;

                if (token == arenaPlayer.ActiveCharacter.Name.ToLower() ||
                    (token == "@chaos" && arenaPlayer.ActiveTeam == Team.Chaos) ||
                    (token == "@balance" && arenaPlayer.ActiveTeam == Team.Balance) ||
                    (token == "@order" && arenaPlayer.ActiveTeam == Team.Order) ||
                    (token == "@neutral" && arenaPlayer.ActiveTeam == Team.Neutral && arenaPlayer.ActiveCharacter.OpLevel == 0) ||
                    (token == "@all" && arenaPlayer.ActiveCharacter.OpLevel == 0))
                {
                    playerList.Add(arenaPlayer);
                }
            }

            return playerList;
        }

        public ListCollection<ArenaPlayer> FindArenaPlayers(Team team)
        {
            ListCollection<ArenaPlayer> playerList = new ListCollection<ArenaPlayer>();

            for (Int32 i = Count - 1; i >= 0; i--)
            {
                ArenaPlayer arenaPlayer = this[i];
                if (arenaPlayer == null) continue;

                if (arenaPlayer.ActiveTeam == team)
                {
                    playerList.Add(arenaPlayer);
                }
            }

            return playerList;
        }

        public ListCollection<ArenaPlayer> FindTop10Points()
        {
            return new ListCollection<ArenaPlayer>(this.Where(arenaPlayer => arenaPlayer.Points > 0).OrderByDescending(arenaPlayer => arenaPlayer.Points).Take(10));
        }

        public Int32 GetAveragePlayerLevel()
        {
            Int32 count = Count;
            if (count == 0) return 1;

            return this.Where(arenaPlayer => arenaPlayer.ActiveCharacter.OpLevel == 0).Aggregate(0, (current, arenaPlayer) => current + arenaPlayer.ActiveCharacter.Level) / count;
        }

        public Int32 GetTeamPlayerCount(Team team)
        {
            return this.Count(p => p.ActiveTeam == team);
        }

		public new void Add(ArenaPlayer item)
		{
			base.Add(item);
			_highestPlayerCount = Count;
		}

		private Int32 _highestPlayerCount;
		public Int32 HighestPlayerCount
		{
			get { return _highestPlayerCount; }
			set
			{
				if (_highestPlayerCount < value)
				{
					_highestPlayerCount = value;
				}
			}
		}
    } 
}
