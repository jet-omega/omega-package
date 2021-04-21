using System;

namespace Omega.Package
{
    public static class PoolsExtension
    {
        public static PoolElementUsageHandler<T> Use<T>(this IPool<T> pool, out T element)
        {
            if (pool is null)
                throw new NullReferenceException(nameof(pool));

            element = pool.Get();
            return new PoolElementUsageHandler<T>(pool, element);
        }
    }
}