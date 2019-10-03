using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Omega.Tools
{
    public static class RectTransformUtility
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
        public static RectTransform[] GetChildes([NotNull] RectTransform root)
        {
            if (ReferenceEquals(root, null))
                throw new ArgumentNullException(nameof(root));
            if (!root)
                throw new MissingReferenceException(nameof(root));

            return GetChildesWithoutChecks(root);
        }

        [NotNull]
        internal static RectTransform[] GetChildesWithoutChecks([NotNull] RectTransform rectTransform)
        {
            var transformChildes = TransformUtility.GetChildesWithoutChecks(rectTransform);

            var rectTransformChildesList = new List<RectTransform>(transformChildes.Length);

            foreach (var child in transformChildes)
                if (child is RectTransform rectTransformChild)
                    rectTransformChildesList.Add(rectTransformChild);

            var rectTransformChildes = rectTransformChildesList.ToArray();

            return rectTransformChildes;
        }
    }
}