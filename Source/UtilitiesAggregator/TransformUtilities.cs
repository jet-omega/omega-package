using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Omega.Package;
using UnityEngine;

namespace Omega.Package.Internal
{
    public class TransformUtilities
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

        public bool IsChildOf([NotNull] Transform transform, [CanBeNull] Transform parent)
        {
            if (transform is null)
                throw new ArgumentNullException(nameof(transform));
            if (!transform)
                throw new MissingReferenceException(nameof(transform));

            return IsChildOfWithoutChecks(transform, parent);
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

        public void GetChilds(Transform root, List<Transform> result)
        {
            if (root is null)
                throw new ArgumentNullException(nameof(root));
            if (!root)
                throw new MissingReferenceException(nameof(root));

            GetChildsWithoutChecks(root, result);
        }

        public void GetAllChilds(Transform root, List<Transform> result)
        {
            if (root is null)
                throw new ArgumentNullException(nameof(root));
            if (!root)
                throw new MissingReferenceException(nameof(root));
            if (result is null)
                throw new ArgumentNullException(nameof(result));

            GetAllChildsWithoutChecks(root, result);
        }

        public int GetAllChildsCount(Transform root)
        {
            if (root is null)
                throw new ArgumentNullException(nameof(root));
            if (!root)
                throw new MissingReferenceException(nameof(root));

            return GetAllChildsCountWithoutChecks(root);
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

        internal static bool IsChildOfWithoutChecks([CanBeNull] Transform transform, [CanBeNull] Transform parent)
        {
            var temp = transform;
            while (temp)
            {
                var tempParent = temp.parent;
                if (tempParent == parent)
                    return true;

                temp = tempParent;
            }

            return false;
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

            ListPool<Transform>.ReturnInternal(childs);
        }

        internal static void GetAllChildsWithoutChecks([NotNull] Transform root, [NotNull] List<Transform> childs)
        {
            var i = childs.Count;
            GetChildsWithoutChecks(root, childs);
            var count = childs.Count;
            for (; i < count; i++)
                GetAllChildsWithoutChecks(childs[i], childs);
        }

        internal static int GetAllChildsCountWithoutChecks([NotNull] Transform root)
        {
            int result = 0;
            var childs = ListPool<Transform>.Rent();
            GetAllChildsWithoutChecks(root, childs);
            result = childs.Count;
            ListPool<Transform>.Return(childs);

            return result;
        }
    }
}