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
        public static RectTransform[] GetChilds([NotNull] RectTransform root)
        {
            if (root is null)
                throw new ArgumentNullException(nameof(root));
            if (!root)
                throw new MissingReferenceException(nameof(root));

            return GetChildsWithoutChecks(root);
        }

        [NotNull]
        internal static RectTransform[] GetChildsWithoutChecks([NotNull] RectTransform rectTransform)
        {
            var transformChilds = TransformUtility.GetChildsWithoutChecks(rectTransform);

            var rectTransformChildsList = new List<RectTransform>(transformChilds.Length);

            foreach (var child in transformChilds)
                if (child is RectTransform rectTransformChild)
                    rectTransformChildsList.Add(rectTransformChild);

            var rectTransformChilds = rectTransformChildsList.ToArray();

            return rectTransformChilds;
        }
    }
}