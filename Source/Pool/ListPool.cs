using System;
using System.Collections.Generic;

namespace Omega.Package
{
    public sealed class ListPool<TElement> : CollectionsPoolBase<List<TElement>, TElement>
    {
        private static ListPool<TElement> _shared;
        private static ListPool<TElement> _internalShared;

        public static ListPool<TElement> Shared
            => _shared = _shared ?? new ListPool<TElement>(7)
            {
                StartCapacity = 7
            };

        internal static ListPool<TElement> InternalShared
            => _internalShared = _internalShared ?? new ListPool<TElement>(1)
            {
                StartCapacity = 15
            };

        private int _startCapacity;

        public int StartCapacity
        {
            get => _startCapacity;
            set => _startCapacity = value < 0 ? throw new ArgumentOutOfRangeException(nameof(value)) : value;
        }

        public List<TElement> Get(int capacity)
        {
            if (!HasPoolElements)
                return new List<TElement>(capacity);

            var list = Get();
            var bestCapacity = Math.Max(list.Capacity, capacity);
            list.Capacity = bestCapacity;
            return list;
        }

        protected override List<TElement> CreateNew()
            => new List<TElement>(StartCapacity);

        public ListPool(int initialPoolCapacity = 3)
            : base(initialPoolCapacity)
        {
        }
    }
}