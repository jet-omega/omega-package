using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Omega.Package.Internal
{
    public sealed class BoxColliderUtilities
    {
        internal BoxColliderUtilities()
        {
        }
        
        /// <summary>
        /// Устанавливает центр и размер коллайдеру по указанным границам  
        /// </summary>
        /// <param name="boxCollider">Коллайдер</param>
        /// <param name="bounds">Границы для коллайдера</param>
        /// <exception cref="ArgumentNullException">Параметр <param name="boxCollider"/>>указывает на null</exception>
        /// <exception cref="MissingReferenceException">Параметр <param name="boxCollider"/>>указывает на уничтоженный объект</exception>
        public void SetAsBounds([NotNull] BoxCollider boxCollider, Bounds bounds)
        {
            if (boxCollider is null)
                throw new ArgumentNullException(nameof(boxCollider));
            if (!boxCollider)
                throw new MissingReferenceException(nameof(boxCollider));

            SetAsBoundsWithoutChecks(boxCollider, bounds);
        }

        internal static void SetAsBoundsWithoutChecks([NotNull] BoxCollider boxCollider, Bounds bounds)
        {
            boxCollider.center = bounds.center;
            boxCollider.size = bounds.size;
        }
    }
}