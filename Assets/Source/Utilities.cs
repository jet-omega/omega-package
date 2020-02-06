using Omega.Package.Internal;

namespace Omega.Package
{
    public static class Utilities
    {
        private static TransformUtilities _transfrom = new TransformUtilities();
        private static GameObjectUtilities _gameObject = new GameObjectUtilities();
        private static RectTransformUtilities _rectTransform = new RectTransformUtilities();
        private static ObjectUtilities _object = new ObjectUtilities();
        private static BoxColliderUtilities _boxCollider = new BoxColliderUtilities();

        public static TransformUtilities Transfrom => _transfrom;
        public static GameObjectUtilities GameObject => _gameObject;
        public static RectTransformUtilities RectTransform => _rectTransform;
        public static ObjectUtilities Object => _object;
        public static BoxColliderUtilities BoxCollider => _boxCollider;
    }
}