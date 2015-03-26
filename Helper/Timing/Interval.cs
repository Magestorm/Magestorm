using System;

namespace Helper.Timing
{
    public class Interval
    {
		private readonly DateTime _creationTime;
        private readonly Boolean _autoReset;
        private readonly Int32 _maxResets;
        private readonly Object _syncRoot = new Object();
        private Int32 _currentResets;
        private Int64 _lastTick;
        private Int64 _duration;
        private Boolean _end;

        public Interval(Int64 milliseconds, Boolean autoReset)
        {
			_creationTime = DateTime.Now;
            _autoReset = autoReset;
            _end = false;
            Duration = milliseconds;
        }

        public Interval(Int64 milliseconds, Int32 maxResets)
        {
	        _creationTime = DateTime.Now;
            _maxResets = maxResets;
            _autoReset = true;
            _end = false;
            Duration = milliseconds;
        }
		public Int64 Duration
		{
			get { return _duration; }
			set
			{
				_duration = value;
				Reset();
			}
		}

		public Object SyncRoot
		{
			get { return _syncRoot; }
		}

		public DateTime CreationTime
		{
			get { return _creationTime; }
		}

        public Boolean HasElapsed
        {
            get
            {
                lock (SyncRoot)
                {
                    if (_duration == 0) return false;

                    Int64 timeStamp = NativeMethods.PerformanceCount;

                    if (TimeHelper.DeltaMilliseconds(_lastTick, timeStamp) >= _duration  || _end)
                    {
                        if (_autoReset && (_maxResets == 0 || _currentResets < _maxResets))
                        {
                            _lastTick = timeStamp;
                            _currentResets++;
                        }

                        _end = false;

                        return true;
                    }

                    return false;
                }
            }
        }

        public Boolean CanReset
        {
            get { return _currentResets < _maxResets || (_maxResets == 0 && _autoReset); }
        }

        public Single Delta
        {
            get { return ElapsedMilliseconds * 0.001f; }
        }

        public Int64 ElapsedMicroseconds
        {
            get { return TimeHelper.DeltaMicroseconds(_lastTick, NativeMethods.PerformanceCount); }
        }

        public Int64 ElapsedMilliseconds
        {
            get { return TimeHelper.DeltaMilliseconds(_lastTick, NativeMethods.PerformanceCount); }
        }

        public Int64 ElapsedSeconds
        {
            get { return TimeHelper.DeltaSeconds(_lastTick, NativeMethods.PerformanceCount); }
        }

        public Int64 RemainingMilliseconds
        {
            get
            {
                Int64 remaining = _duration - ElapsedMilliseconds;
                return remaining < 0 ? 0 : remaining;
            }
        }

        public Int64 RemainingSeconds
        {
            get
            {
                Int64 remaining = (Int64)(_duration * 0.001) - ElapsedSeconds;
                return remaining < 0 ? 0 : remaining;
            }
        }

        public void Reset()
        {
            lock (SyncRoot)
            {
                _lastTick = NativeMethods.PerformanceCount;
            }
        }

        public void End()
        {
            lock (SyncRoot)
            {
                _end = true;
            }
        }
    }
}
