using Omega.Tools.Experimental.UtilitiesAggregator;

namespace Omega.Experimental
{
    public static class Utilities
    {
        private static TransfromUtilites _transfrom = new TransfromUtilites();
        private static GameObjectUtilities _gameObject = new GameObjectUtilities();
        private static RectTransformUtilities _rectTransform = new RectTransformUtilities();
        private static TimeUtilities _time = new TimeUtilities();

        public static TransfromUtilites Transfrom => _transfrom;
        public static GameObjectUtilities GameObject => _gameObject;
        public static RectTransformUtilities RectTransform => _rectTransform;
        public static TimeUtilities Time => _time;
    }
}