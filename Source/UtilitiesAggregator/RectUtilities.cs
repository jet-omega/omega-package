using UnityEngine;

namespace Omega.Package.Internal
{
    public sealed class RectUtilities
    {
        internal RectUtilities()
        {
        }

        public Rect BetweenTwoPoints(Vector2 first, Vector2 second)
        {
            return Rect.MinMaxRect(
                Mathf.Min(first.x, second.x),
                Mathf.Min(first.y, second.y),
                Mathf.Max(first.x, second.x),
                Mathf.Max(first.y, second.y));
        }
    }
}