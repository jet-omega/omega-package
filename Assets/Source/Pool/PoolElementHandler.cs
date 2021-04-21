using System;
using JetBrains.Annotations;

namespace Omega.Package
{
    public struct PoolElementUsageHandler<TElement> : IDisposable
    {
        private bool _used;
        private IPool<TElement> _pool;
        private TElement _element;

        public PoolElementUsageHandler(IPool<TElement> pool, TElement element)
        {
            _used = false;
            _pool = pool;
            _element = element;
        }

        public void Dispose()
        {
            if (_used)
                FailAlreadyDisposed();

            _used = true;
            _pool.Return(_element);
        }

        [TerminatesProgram]
        private static void FailAlreadyDisposed()
            => throw new InvalidOperationException();
    }
}