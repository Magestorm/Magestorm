using System;
using System.Windows.Forms;

namespace Helper.Timing
{
    public static class TimeHelper
    {
        private static readonly Int64 Freq = NativeMethods.PerformanceFrequency;

        public static Int64 DeltaMicroseconds(Int64 earlyTimestamp, Int64 lateTimestamp)
        {
            return Freq != 0 ? ((lateTimestamp - earlyTimestamp) * 1000000) / Freq : 0;
        }

        public static Int64 DeltaMilliseconds(Int64 earlyTimestamp, Int64 lateTimestamp)
        {
            return Freq != 0 ? ((lateTimestamp - earlyTimestamp) * 1000) / Freq : 0;
        }

        public static Int64 DeltaSeconds(Int64 earlyTimestamp, Int64 lateTimestamp)
        {
            return Freq != 0 ? (lateTimestamp - earlyTimestamp) / Freq : 0;
        }

        public static Int32 GetStartOfWeekUnixTime()
        {
            return DateTime.UtcNow.StartOfWeek().GetUnixTime();
        }

		public static DateTime UnixTimeToDateTime(Int32 unixTime, Boolean localTime)
		{
			if (localTime)
			{
				return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTime).ToLocalTime();
			}

			return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTime);			
		}

        public static void SpinApplication(Int64 milliseconds)
        {
            Interval delayInterval = new Interval(milliseconds, false);
            while (!delayInterval.HasElapsed)
            {
                Application.DoEvents();
            }
        }
    }
}