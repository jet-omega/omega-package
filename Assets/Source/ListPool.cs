using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace Omega.Package
{
    internal static class ListPool<T>
    {
        private const int DefaultListCapacity = 20;
        [NotNull] private static Stack<List<T>> _pool = new Stack<List<T>>(2);

        [NotNull]
        public static List<T> Rent()
        {
            return _pool.Count == 0
                ? new List<T>(DefaultListCapacity)
                : _pool.Pop();
        }

        public static List<T> Rent(int capacity)
        {
            if (_pool.Count == 0)
            {
                var bestCapacity = Math.Max(DefaultListCapacity, capacity);
                return new List<T>(bestCapacity);
            }

            var list = _pool.Pop();
            if (list.Capacity < capacity)
                list.Capacity = capacity;

            return list;
        }

        public static void Push([NotNull] List<T> list)
        {
            list.Clear();
            _pool.Push(list);
        }
    }
}