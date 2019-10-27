namespace Omega.Tools.Experimental.UtilitiesAggregator
{
    public sealed class TimeUtilities
    {
        internal TimeUtilities()
        {
        }

        public static float FromMilliseconds(float milliseconds)
        {
            return milliseconds / 1000;
        }

        public static float FromSeconds(float seconds)
        {
            return seconds;
        }

        public static float FromMinutes(float minutes)
        {
            return minutes * 60;
        }
    }
}