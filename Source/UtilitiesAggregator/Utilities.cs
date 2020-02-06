using System;
using Omega.Package.Internal;

namespace Omega.Experimental
{
    [Obsolete("Use Utilities from Omega.Package namespace")]
    public static class Utilities
    {
        private static TransformUtilities _transfrom = new TransformUtilities();
        private static GameObjectUtilities _gameObject = new GameObjectUtilities();
        private static RectTransformUtilities _rectTransform = new RectTransformUtilities();
        private static TimeUtilities _time = new TimeUtilities();
        private static ObjectUtilities _object = new ObjectUtilities();
        private static BoxColliderUtilities _boxCollider = new BoxColliderUtilities();

        public static TransformUtilities Transfrom => _transfrom;
        public static GameObjectUtilities GameObject => _gameObject;
        public static RectTransformUtilities RectTransform => _rectTransform;
        public static TimeUtilities Time => _time;
        public static ObjectUtilities Object => _object;
        public static BoxColliderUtilities BoxCollider => _boxCollider;
    }
}