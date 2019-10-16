using Omega.Tools.Experimental.UtilitiesAggregator;

namespace Omega.Experimental
{
    public static class Utilities
    {
        private static TransfromUtilites _transfrom = new TransfromUtilites();
        private static GameObjectUtilities _gameObject = new GameObjectUtilities();
        private static RectTransformUtilities _rectTransform = new RectTransformUtilities();

        public static TransfromUtilites Transfrom => _transfrom;
        public static GameObjectUtilities GameObject => _gameObject;
        public static RectTransformUtilities RectTransform => _rectTransform;
    }
}