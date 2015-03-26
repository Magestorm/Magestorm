using System;

namespace Helper
{
    public class Pool
    {
        public readonly Object SyncRoot = new Object();

        public Int16 Power;
        public Int16 MaxBias;
        public Byte PoolId;
        public Team Team;
        private Int16 _currentBias;

        public Pool(Byte poolId, Int16 power, Int16 maxBias)
        {
            PoolId = poolId;
            Team = Team.Neutral;
            MaxBias = maxBias;
            CurrentBias = 0;
            Power = power;
        }

        public Pool(Pool p)
        {
            PoolId = p.PoolId;
            Team = Team.Neutral;
            MaxBias = p.MaxBias;
            CurrentBias = 0;
            Power = p.Power;
        }

        public Int16 CurrentBias
        {
            get { return _currentBias; }
            set
            {
                if (value < 0) value = 0;
                if (value > 100) value = 100;

                _currentBias = value;
            }
        }
    } 
}
