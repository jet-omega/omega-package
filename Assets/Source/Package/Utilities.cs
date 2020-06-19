using Omega.Package.Internal;
using Omega.Tools;

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
        public static ArrayUtilities Array { get; } = new ArrayUtilities();
        public static LayerUtilities Layer { get; } = new LayerUtilities();
        public static BlockerUtilities Blocker { get; } = new BlockerUtilities();
    }
}