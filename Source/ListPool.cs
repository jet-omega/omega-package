using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Omega.Package
{
    public static class ListPool<T>
    {
        public const int DefaultListCapacity = 20;
        [NotNull] private static readonly Stack<List<T>> Pool = new Stack<List<T>>(2);

        [NotNull]
        public static List<T> Rent()
        {
            return Pool.Count == 0
                ? new List<T>(DefaultListCapacity)
                : Pool.Pop();
        }
        public static List<T> Rent(int requireCapacity)
        {
            if (Pool.Count == 0)
            {
                var bestCapacity = Math.Max(DefaultListCapacity, requireCapacity);
                return new List<T>(bestCapacity);
            }

            var list = Pool.Pop();
            if (list.Capacity < requireCapacity)
                list.Capacity = requireCapacity;

            return list;
        }

        public static void Push([NotNull] List<T> list)
        {
            if(list is null)
                throw new ArgumentNullException(nameof(list));
            
            PushInternal(list);
        }
        internal static void PushInternal(List<T> list)
        {
            list.Clear();
            Pool.Push(list);
        }
    }
}