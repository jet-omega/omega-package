using Omega.Tools;
using Omega.Tools.Experimental.UtilitiesAggregator;

namespace Omega.Experimental
{
    public static class Utilities
    {
        private static TransformUtilities _transfrom = new TransformUtilities();
        private static GameObjectUtilities _gameObject = new GameObjectUtilities();
        private static RectTransformUtilities _rectTransform = new RectTransformUtilities();
        private static TimeUtilities _time = new TimeUtilities();
        private static ObjectUtilities _object = new ObjectUtilities();
        private static BoxColliderUtilities _boxCollider = new BoxColliderUtilities();
        private static ArrayUtilities _arrayUtilities = new ArrayUtilities();

        public static TransformUtilities Transfrom => _transfrom;
        public static GameObjectUtilities GameObject => _gameObject;
        public static RectTransformUtilities RectTransform => _rectTransform;
        public static TimeUtilities Time => _time;
        public static ObjectUtilities Object => _object;
        public static BoxColliderUtilities BoxCollider => _boxCollider;
        public static ArrayUtilities Array => _arrayUtilities;
    }
}