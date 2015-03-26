using System;
using System.Linq;

namespace Helper
{
    public class PoolCollection : ListCollection<Pool>
    {
        public Pool FindById(Int16 poolId)
        {
            return this.FirstOrDefault(p => poolId == p.PoolId);
        }
        public Int32 GetTeamPoolCount(Team team, Boolean findOnlyFullyBiased)
        {
            return this.Count(p => p.Team == team && p.CurrentBias == p.MaxBias);
        }
    }
}
