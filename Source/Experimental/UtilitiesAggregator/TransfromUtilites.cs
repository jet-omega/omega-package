using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Omega.Tools.Experimental.UtilitiesAggregator
{
    public sealed class TransfromUtilites
    {
        internal TransfromUtilites()
        {
        }
        
        /// <summary>
        /// Возвращает всех потомков указанного трансформа 
        /// </summary>
        /// <param name="root">Трансформ, относительно которого будет осуществляться поиск потомков </param>
        /// <returns>Массив потомков</returns>
        /// <exception cref="ArgumentNullException">Параметр <param name="root"/>>указывает на null</exception>
        /// <exception cref="MissingReferenceException">Параметр <param name="root"/>>указывает на уничтоженный объект</exception>
        [NotNull]
        public Transform[] GetChilds([NotNull] Transform root)
        {
            if (ReferenceEquals(root, null))
                throw new ArgumentNullException(nameof(root));
            if (!root)
                throw new MissingReferenceException(nameof(root));

            return GetChildsWithoutChecks(root);
        }

        [NotNull]
        internal Transform[] GetChildsWithoutChecks([NotNull] Transform root)
        {
            var childsCount = root.childCount;
            if (childsCount == 0)
                return Array.Empty<Transform>();

            var childs = new Transform[childsCount];
            for (int i = 0; i < childs.Length; i++)
                childs[i] = root.GetChild(i);

            return childs;
        }
    }
}