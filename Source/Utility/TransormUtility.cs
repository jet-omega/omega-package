using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Omega.Tools
{
    public static class TransformUtility
    {
        /// <summary>
        /// Возвращает всех потомков указанного трансформа 
        /// </summary>
        /// <param name="root">Трансформ, относительно которого будет осуществляться поиск потомков </param>
        /// <returns>Массив потомков</returns>
        /// <exception cref="ArgumentNullException">Параметр <param name="root"/>>указывает на null</exception>
        /// <exception cref="MissingReferenceException">Параметр <param name="root"/>>указывает на уничтоженный объект</exception>
        [NotNull]
        public static Transform[] GetChildes([NotNull] Transform root)
        {
            if (ReferenceEquals(root, null))
                throw new ArgumentNullException(nameof(root));
            if (!root)
                throw new MissingReferenceException(nameof(root));

            return GetChildesWithoutChecks(root);
        }

        [NotNull]
        internal static Transform[] GetChildesWithoutChecks([NotNull] Transform root)
        {
            var childesCount = root.childCount;
            if (childesCount == 0)
                return Array.Empty<Transform>();

            var childes = new Transform[childesCount];
            for (int i = 0; i < childes.Length; i++)
                childes[i] = root.GetChild(i);

            return childes;
        }
    }
}