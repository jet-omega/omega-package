using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Omega.Package;
using UnityEngine;

namespace Omega.Package.Internal
{
    public sealed class RectTransformUtilities : TransformUtilities
    {
        /// <summary>
        /// Возвращает всех потомков указанного трансформа, если потомок этого трансформа не кастится к RectTransfrom,
        /// то он не будет добавлен в конечный массив  
        /// </summary>
        /// <param name="root">Трансформ, относительно которого будет осуществляться поиск потомков</param>
        /// <returns>Массив потомков, кроме тех которые не являются производными от RectTransform</returns>
        /// <exception cref="ArgumentNullException">Параметр <param name="root"/>>указывает на null</exception>
        /// <exception cref="MissingReferenceException">Параметр <param name="root"/>>указывает на уничтоженный объект</exception>
        [NotNull]
        public RectTransform[] GetChildren([NotNull] RectTransform root)
        {
            if (root is null)
                throw new ArgumentNullException(nameof(root));
            if (!root)
                throw new MissingReferenceException(nameof(root));

            if (root.childCount == 0)
                return Array.Empty<RectTransform>();

            using (ListPool<RectTransform>.InternalShared.Use(out var list))
            {
                GetChildrenWithoutChecks(root, list);
                return list.ToArray();
            }
        }

        internal static void SetRectWithoutChecks([NotNull] RectTransform rectTransform, Rect rect)
        {
            var z = rectTransform.position.z;

            var size = rect.size;
            var offset = size * rectTransform.pivot;
            var position = new Vector3(rect.x + offset.x, rect.y + offset.y, z);

            rectTransform.position = position;
            rectTransform.sizeDelta = size;
        }

        internal static void GetChildrenWithoutChecks([NotNull] RectTransform rectTransform,
            [NotNull] List<RectTransform> result)
        {
            var childrenCount = rectTransform.childCount;
            for (var i = 0; i < childrenCount; i++)
            {
                var child = rectTransform.GetChild(i);
                if (child is RectTransform rectTransformChild)
                    result.Add(rectTransformChild);
            }
        }
    }
}