using System;
using Helper.Timing;

namespace Helper
{
    public enum Team
    {
        Neutral,
        Chaos,
        Order,
        Balance,
        None,
    }

    public class Shrine
    {
        public readonly Object SyncRoot = new Object();

        public Int16 Power;
        public Int16 MaxBias;
        public Byte ShrineId;
        public Team Team;
        public Interval GuildPointTime;

        private Boolean _isDisabled;
        private Int16 _currentBias;
        private Single _guildPoints;

        public Boolean IsDisabled
        {
            get { return _isDisabled; }
            set
            {
                if (value)
                {
                    _currentBias = 0;
                }

                _isDisabled = value;
            }
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


        public Single GuildPoints
        {
            get { return _guildPoints; }
            set
            {
                if (GuildPointTime.HasElapsed)
                {
                    _guildPoints = value;
                }
            }
        }

        public Boolean IsDead
        {
            get
            {
                return CurrentBias == 0 || IsDisabled;
            }
        }

        public Boolean IsDamaged
        {
            get
            {
                return CurrentBias < 100;
            }
        }

        public Boolean IsIndestructible
        {
            get
            {
                return Power == -1;
            }
        }

        public Shrine(Team shrineTeam, Byte shrineId, Int16 power, Int16 bias)
        {
            _guildPoints = 0;
            GuildPointTime = new Interval(1000, true);
            ShrineId = shrineId;
            Team = shrineTeam;
            MaxBias = 100;
            CurrentBias = bias;
            Power = power;
            IsDisabled = false;
        }
    }
}
