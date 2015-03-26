using System;
using Helper;

namespace MageServer
{
    [Flags]
    public enum GridBlockFlag
    {
        None = 0x0,
        Valhalla = 0xD2,
        ManaPool = 0x78,
        Shrine = 0x96,
    }

    public class GridBlockFlagData
    {
        public GridBlockFlag BlockFlag;
        public Pool Pool;
        public ArenaTeam ShrineTeam;

        public GridBlockFlagData()
        {
            BlockFlag = GridBlockFlag.None;
            ShrineTeam = null;
        }

        public void UpdateFlagData(Arena arena, Int32 flagData)
        {
            if (flagData == (Int32)GridBlockFlag.Valhalla)
            {
                Pool = null;
                ShrineTeam = null;
                BlockFlag = GridBlockFlag.Valhalla;
                return;
            }

            if (flagData >= (Int32)GridBlockFlag.ManaPool && flagData < (Int32)GridBlockFlag.Shrine)
            {
                flagData -= (Int32)GridBlockFlag.ManaPool;

                ShrineTeam = null;
                Pool = arena.Grid.Pools.FindById((Int16) flagData);
                BlockFlag = Pool == null ? GridBlockFlag.None : GridBlockFlag.ManaPool;
                
                return;
            }

            if (flagData >= (Int32)GridBlockFlag.Shrine && flagData < ((Int32)GridBlockFlag.Shrine + 3))
            {
                
                flagData -= (Int32)GridBlockFlag.Shrine;

                ShrineTeam = arena.ArenaTeams.FindByShrineId((Byte) flagData);
                Pool = null;
                BlockFlag = ShrineTeam == null ? GridBlockFlag.None : GridBlockFlag.Shrine;

                return;
            }

            Pool = null;
            ShrineTeam = null;
            BlockFlag = GridBlockFlag.None;
        }
    }
}
