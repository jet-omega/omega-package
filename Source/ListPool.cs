using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Omega.Package
{
    public static class ListPool<T>
    {
        public const int DefaultListCapacity = 10;
        [NotNull] private static readonly Stack<List<T>> Pool = new Stack<List<T>>(2);

        public static int PoolSize => Pool.Count;
        
        [NotNull]
        public static List<T> Rent()
        {
            return Pool.Count is 0
                ? new List<T>(DefaultListCapacity)
                : Pool.Pop();
        }
        public static List<T> Rent(int requireCapacity)
        {
            if (Pool.Count is 0)
            {
                var bestCapacity = Math.Max(requireCapacity, DefaultListCapacity);
                return new List<T>(bestCapacity);
            }

            var list = Pool.Pop();
            if (list.Capacity < requireCapacity)
                list.Capacity = requireCapacity;

            return list;
        }

        public static ListPoolHandler<T> Get(out List<T> list)
        {
            list = Rent();
            return new ListPoolHandler<T>(list);
        }
        
        [Obsolete("Use Return method")]
        public static void Push([NotNull] List<T> list)
        {
            Return(list);
        }

        public static void Flush()
        {
            Pool.Clear();
            Pool.TrimExcess();
        }
        
        
        public static void Return([NotNull] List<T> list)
        {
            if(list is null)
                throw new ArgumentNullException(nameof(list));
            
            ReturnInternal(list);
        }

        public static T[] ReturnToArray([NotNull] List<T> list)
        {
            if(list is null)
                throw new ArgumentNullException(nameof(list));

            var array = list.ToArray();
            ReturnInternal(list);
            return array;
        }

        internal static void ReturnInternal(List<T> list)
        {
            list.Clear();
            Pool.Push(list);
        }
    }
}