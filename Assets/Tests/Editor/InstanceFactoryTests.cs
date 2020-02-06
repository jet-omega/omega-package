using System;
using System.Collections.Generic;
using NUnit.Framework;
using Omega.Package;

namespace Omega.Tools.Tests
{
    public class InstanceFactoryTests
    {
        [Test]
        public void CreateShouldInstantiatePrimitivesTest() => InstantiateTest<float>();

        [Test]
        public void CreateShouldInstantiateValueTypesTest() => InstantiateTest<TimeSpan>();
        
        [Test]
        public void CreateShouldInstantiateReferenceTypesTest() => InstantiateTest<TestClass>();

        private void InstantiateTest<T>()
        {
            var type = typeof(T);
            var defaultValue = Activator.CreateInstance<T>();
            var newValue = InstanceFactory.Create(type);

            Assert.AreEqual(defaultValue, newValue);
        }

        private class TestClass
        {
            private float foo;
            private List<float> bar;

            public TestClass()
            {
                foo = 334;
                bar = null;
            }

            protected bool Equals(TestClass other)
            {
                return foo.Equals(other.foo) && Equals(bar, other.bar);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((TestClass) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (foo.GetHashCode() * 397) ^ (bar != null ? bar.GetHashCode() : 0);
                }
            }
        }
    }
}