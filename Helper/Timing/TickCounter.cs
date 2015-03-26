using System;

namespace Helper.Timing
{
    public class TickCounter
    {
        private readonly Interval _tickInterval;

        public Int64 Ticks
        {
            get
            {
                Int64 tickCount = _tickInterval.ElapsedMilliseconds / MillisecondsPerTick;

                if (tickCount > TickMaxCount) tickCount = TickMaxCount;

                return tickCount;
            }
        }

        public readonly Int32 TickMaxCount;
        public readonly Int32 MillisecondsPerTick;

        public TickCounter(Int32 tickMaxCount, Int32 msPerTick)
        {
            if (tickMaxCount <= 0) tickMaxCount = 1;
            if (msPerTick <= 0) msPerTick = 1;

            TickMaxCount = tickMaxCount;
            MillisecondsPerTick = msPerTick;

            _tickInterval = new Interval(0, false);
        }
    }
}
