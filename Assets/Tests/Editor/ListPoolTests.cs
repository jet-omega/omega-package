using System.Collections.Generic;
using NUnit.Framework;

namespace Omega.Package.Tests.Editor
{
    public class ListPoolTests
    {
        [Test]
        public void GetMethodShouldRentListFromPool()
        {
            ListPool<float>.Return(new List<float>(ListPool<float>.DefaultListCapacity));

            ListPool<float>.Get(out var list);

            Assert.Zero(ListPool<float>.PoolSize);
        }

        [Test]
        public void ReturnMethodShouldPushListToPool()
        {
            ListPool<float>.Return(new List<float>(10));
            Assert.AreEqual(1, ListPool<float>.PoolSize);
        }

        [Test]
        public void ListPoolHandlerShouldReturnListToPool()
        {
            var handler = ListPool<float>.Get(out var list);
            handler.Dispose();
            Assert.AreEqual(1, ListPool<float>.PoolSize);
        }

        [SetUp]
        public void FlushListPoolOfFloat()
            => ListPool<float>.Flush(); // clean pool
    }
}