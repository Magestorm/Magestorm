using System;
using System.Linq;
using Helper;

namespace MageServer
{
    public class PlayerManager : ListCollection<Player>
    {
        public static readonly PlayerManager Players = new PlayerManager();

        public Int16 AvailableId
        {
            get
            {
                for (Int16 i = 1; i <= 510; i++)
                {
                    if (FindById(i) == null)
                    {
                        return i;
                    }
                }

                return 0;
            }
        }

        public Player FindById(Int16 playerId)
        {
            return this.FirstOrDefault(p => playerId == p.PlayerId);
        }

        public Player FindByAccountId(Int32 accountId)
        {
            return this.FirstOrDefault(p => accountId == p.AccountId);
        }

        public Player FindByUsername(String name)
        {
            return this.FirstOrDefault(p => name.ToLower() == p.Username.ToLower());
        }

        public Player FindByCharacterName(String name)
        {
            return this.FirstOrDefault(p => p.ActiveCharacter != null && name.ToLower() == p.ActiveCharacter.Name.ToLower());
        }

        public Player FindBySerial(String serial)
        {
            return this.FirstOrDefault(p => serial == p.Serial);
        }

        public Int32 GetFreePlayerCount()
        {
            return this.Count(p => !p.Flags.HasFlag(PlayerFlag.MagestormPlus));
        }
    }
}