using System;

namespace Omega.Package
{
    internal struct TimeMeter
    {
        private DateTime _initial;

        public static TimeMeter New()
        {
            var timeMeter = new TimeMeter();
            timeMeter.Start();
            return timeMeter;
        }

        public void Start()
        {
            _initial = DateTime.UtcNow;
        }

        public TimeSpan ToSpan()
        {
            if (_initial == default)
                throw new InvalidOperationException("Firstly need call Start");

            var delta = _initial - DateTime.UtcNow;
            return delta;
        }

        public float ToSeconds()
        {
            var elapsed = ToSpan();
            return (float) elapsed.TotalSeconds;
        }
        
        public float ToMilliseconds()
        {
            var elapsed = ToSpan();
            return (float) elapsed.TotalMilliseconds;
        } 
    }
}