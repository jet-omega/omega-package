using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Omega.Package;
using UnityEngine;

namespace Omega.Tools.Experimental.UtilitiesAggregator
{
    public sealed class TransformUtilities
    {
        internal TransformUtilities()
        {
        }

        /// <summary>
        /// Уничтожает всех потомков переданного трансформа
        /// </summary>
        /// <param name="root">Трансформ, потомки которого будут удалены</param>
        /// <exception cref="ArgumentNullException">Параметр <param name="root"/>>указывает на null</exception>
        /// <exception cref="MissingReferenceException">Параметр <param name="root"/>>указывает на уничтоженный объект</exception>
        public void ClearChilds([NotNull] Transform root)
        {
            if (root is null)
                throw new ArgumentNullException(nameof(root));
            if (!root)
                throw new MissingReferenceException(nameof(root));

            ClearChildsWithoutChecks(root);
        }
        
        /// <summary>
        /// Возвращает всех потомков переданного трансформа 
        /// </summary>
        /// <param name="root">Трансформ, относительно которого будет осуществляться поиск потомков </param>
        /// <returns>Массив потомков</returns>
        /// <exception cref="ArgumentNullException">Параметр <param name="root"/>>указывает на null</exception>
        /// <exception cref="MissingReferenceException">Параметр <param name="root"/>>указывает на уничтоженный объект</exception>
        [NotNull]
        public Transform[] GetChilds([NotNull] Transform root)
        {
            if (root is null)
                throw new ArgumentNullException(nameof(root));
            if (!root)
                throw new MissingReferenceException(nameof(root));

            return GetChildsWithoutChecks(root);
        }
        
        [NotNull]
        internal static Transform[] GetChildsWithoutChecks([NotNull] Transform root)
        {
            var childsCount = root.childCount;
            if (childsCount == 0)
                return Array.Empty<Transform>();

            var childs = new Transform[childsCount];
            for (int i = 0; i < childs.Length; i++)
                childs[i] = root.GetChild(i);

            return childs;
        }

        internal static void GetChildsWithoutChecks([NotNull] Transform root, [NotNull] List<Transform> result)
        {
            var childsCount = root.childCount;
            for (int i = 0; i < childsCount; i++)
                result.Add(root.GetChild(i));
        }
        
        internal static void ClearChildsWithoutChecks([NotNull] Transform root)
        {
            var childesCount = root.childCount;
            if (childesCount == 0)
                return;
            
            var childs = ListPool<Transform>.Rent(childesCount);
            GetChildsWithoutChecks(root, childs);
            
            foreach (var child in childs)
                ObjectUtilities.AutoDestroyWithoutChecks(child.gameObject);
            
            ListPool<Transform>.PushInternal(childs);
        }
    }
}