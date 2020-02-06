using Omega.Package.Internal;

namespace Omega.Package
{
    public static class Utilities
    {
        public static TransformUtilities Transform { get; } = new TransformUtilities();
        public static GameObjectUtilities GameObject { get; } = new GameObjectUtilities();
        public static RectTransformUtilities RectTransform { get; } = new RectTransformUtilities();
        public static RectUtilities Rect { get; } = new RectUtilities();
        public static ObjectUtilities Object { get; } = new ObjectUtilities();
        public static BoxColliderUtilities BoxCollider { get; } = new BoxColliderUtilities();
    }
}