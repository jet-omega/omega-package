using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Omega.Tools.Experimental.UtilitiesAggregator
{
    public sealed class RectTransformUtilities
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
        public RectTransform[] GetChilds([NotNull] RectTransform root)
        {
            if (ReferenceEquals(root, null))
                throw new ArgumentNullException(nameof(root));
            if (!root)
                throw new MissingReferenceException(nameof(root));

            return GetChildsWithoutChecks(root);
        }

        [NotNull]
        internal RectTransform[] GetChildsWithoutChecks([NotNull] RectTransform rectTransform)
        {
            var transformChilds = TransformUtilities.GetChildsWithoutChecks(rectTransform);

            if (transformChilds.Length == 0)
                return Array.Empty<RectTransform>();
            
            var rectTransformChildsList = new List<RectTransform>(transformChilds.Length);

            for (var i = 0; i < transformChilds.Length; i++)
            {
                var child = transformChilds[i];
                if (child is RectTransform rectTransformChild)
                    rectTransformChildsList.Add(rectTransformChild);
            }

            var rectTransformChilds = rectTransformChildsList.ToArray();

            return rectTransformChilds;
        }
    }
}