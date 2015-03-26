using System;
using Helper;

namespace MageServer
{
    public class ArenaTeam
    {
        public Shrine Shrine;
        public CTFOrb ShrineOrb;
        
        public ArenaTeam(Shrine shrine)
        {
            Shrine = shrine;

            Int16 objectId = 0;

            switch (Shrine.Team)
            {
                case Team.Chaos:
                {
                    objectId = 28000;
                    break;
                }
                case Team.Order:
                {
                    objectId = 28001;
                    break;
                }
                case Team.Balance:
                {
                    objectId = 28002;
                    break;
                }
            }

            ShrineOrb = new CTFOrb(Shrine.Team, objectId);
        }
    }
}
