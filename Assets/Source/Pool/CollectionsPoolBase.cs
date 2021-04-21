using System.Collections.Generic;

namespace Omega.Package
{
    public abstract class CollectionsPoolBase<TCollection, TElement> : IPool<TCollection>
        where TCollection : ICollection<TElement>
    {
        private readonly List<TCollection> _collectionsPool;

        protected bool HasPoolElements => _collectionsPool.Count != 0;

        protected abstract TCollection CreateNew();

        public void Return(TCollection collection)
        {
            collection.Clear();
            _collectionsPool.Add(collection);
        }

        public TCollection Get()
        {
            var elementsCount = _collectionsPool.Count;

            if (elementsCount is 0)
                return CreateNew();

            var elementIndex = elementsCount - 1;
            var element = _collectionsPool[elementIndex];
            _collectionsPool.RemoveAt(elementIndex);
            return element;
        }

        protected CollectionsPoolBase(int initialPoolCapacity) 
            => _collectionsPool = new List<TCollection>(initialPoolCapacity);
    }
}