using System;

namespace Omega.Package.Internal
{
    [Obsolete]
    public sealed class TimeUtilities
    {
        internal TimeUtilities()
        {
        }

        public float FromMilliseconds(float milliseconds)
        {
            return milliseconds / 1000;
        }

        public float FromSeconds(float seconds)
        {
            return seconds;
        }

        public float FromMinutes(float minutes)
        {
            return minutes * 60;
        }
    }
}