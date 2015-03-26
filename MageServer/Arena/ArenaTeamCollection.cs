using System;
using System.Linq;
using Helper;

namespace MageServer
{
    public class ArenaTeamCollection : ListCollection<ArenaTeam>
    {
        public ArenaTeam Chaos
        {
            get
            {
                return this[0];
            }
        }

        public ArenaTeam Order
        {
            get
            {
                return this[1];
            }
        }

        public ArenaTeam Balance
        {
            get
            {
                return this[2];
            }
        }

        public Int32 DisabledShrineCount
        {
            get
            {
                return this.Count(arenaTeam => arenaTeam.Shrine.IsDisabled);
            }
        }

        public ArenaTeamCollection(Grid grid)
        {
            Add(new ArenaTeam(grid.ChaosShrine));
            Add(new ArenaTeam(grid.OrderShrine));
            Add(new ArenaTeam(grid.BalanceShrine));
        }

        public ArenaTeam FindByTeam(Team team)
        {
            return this.FirstOrDefault(arenaTeam => team == arenaTeam.Shrine.Team);
        }

        public ArenaTeam FindByShrine(Shrine shrine)
        {
            return this.FirstOrDefault(arenaTeam => shrine == arenaTeam.Shrine);
        }

        public ArenaTeam FindByShrineId(Byte shrineId)
        {
            return this.FirstOrDefault(arenaTeam => shrineId == arenaTeam.Shrine.ShrineId);
        }

        public Boolean IsPlayerCarryingOrb(ArenaPlayer arenaPlayer)
        {
            return this.Any(arenaTeam => arenaPlayer == arenaTeam.ShrineOrb.OrbPlayer);
        }

        public ArenaTeam GetCarriedOrbTeam(ArenaPlayer arenaPlayer)
        {
            return this.FirstOrDefault(arenaTeam => arenaPlayer == arenaTeam.ShrineOrb.OrbPlayer);
        }
    }
}
