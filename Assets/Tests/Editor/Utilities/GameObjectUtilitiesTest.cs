using NUnit.Framework;
using Omega.Package;
using UnityEngine;

namespace Omega.Tools.Tests
{
    public class GameObjectUtilitiesTest
    {
        [Test]
        public void GetComponentInDirectChildrenShouldGetComponentInChildesTest()
        {
            var instance = new GameObject();

            var child = GameObjectFactory.New()
                .SetParent(instance.transform, true)
                .AddComponent<TestComponent>()
                .Build();

            var component = Utilities.GameObject.GetComponentInDirectChildren<TestComponent>(instance);

            Assert.NotNull(component);

            Utilities.Object.AutoDestroy(instance);
        }

        [Test]
        public void GetComponentInDirectChildrenShouldGetComponentInChildesWithoutGenericTest()
        {
            var instance = new GameObject();

            var child = GameObjectFactory.New()
                .SetParent(instance.transform, true)
                .AddComponent<TestComponent>()
                .Build();

            var componentType = typeof(TestComponent);

            var component = Utilities.GameObject.GetComponentInDirectChildren(instance, componentType);

            Assert.NotNull(component);

            Utilities.Object.AutoDestroy(instance);
        }

        [Test]
        public void GetComponentsInDirectChildrenShouldGetComponentsInChildesTest()
        {
            var instance = new GameObject();

            var countComponentsPerObject = 5;
            var countObject = 10;

            GameObjectFactory.New()
                .SetParent(instance.transform, true)
                .Custom(e =>
                {
                    for (int i = 0; i < countComponentsPerObject; i++)
                        e.AddComponent<TestComponent>();
                })
                .Build(countObject);

            var components = Utilities.GameObject.GetComponentsInDirectChildren<TestComponent>(instance);

            Assert.AreEqual(countComponentsPerObject * countObject, components.Length);

            Utilities.Object.AutoDestroy(instance);
        }

        [Test]
        public void GetComponentsInDirectChildrenShouldGetComponentsInChildesWithoutGenericTest()
        {
            var instance = new GameObject();

            var countComponentsPerObject = 5;
            var countObject = 10;

            GameObjectFactory.New()
                .SetParent(instance.transform, true)
                .Custom(e =>
                {
                    for (int i = 0; i < countComponentsPerObject; i++)
                        e.AddComponent<TestComponent>();
                })
                .Build(countObject);

            var componentType = typeof(TestComponent);

            var components = Utilities.GameObject.GetComponentsInDirectChildren(instance, componentType);

            Assert.AreEqual(countComponentsPerObject * countObject, components.Length);

            Utilities.Object.AutoDestroy(instance);
        }

        [Test]
        public void GetComponentInDirectChildrenShouldNotGetComponentInRootTest()
        {
            var instance = new GameObject();

            var componentOnInstance = instance.AddComponent<TestComponent>();

            var child = GameObjectFactory.New()
                .SetParent(instance.transform, true)
                .AddComponent<TestComponent>()
                .Build();

            var component = Utilities.GameObject.GetComponentInDirectChildren<TestComponent>(instance);

            Assert.AreNotEqual(componentOnInstance, component);

            Utilities.Object.AutoDestroy(instance);
        }

        [Test]
        public void GetComponentInDirectChildrenShouldNotGetComponentInRootWithoutGenericTest()
        {
            var instance = new GameObject();

            var componentOnInstance = instance.AddComponent<TestComponent>();

            var child = GameObjectFactory.New()
                .SetParent(instance.transform, true)
                .AddComponent<TestComponent>()
                .Build();

            var componentType = typeof(TestComponent);

            var component = Utilities.GameObject.GetComponentInDirectChildren(instance, componentType);

            Assert.AreNotEqual(componentOnInstance, component);

            Utilities.Object.AutoDestroy(instance);
        }

        [Test]
        public void GetComponentInDirectChildrenShouldGetComponentInRootTest()
        {
            var instance = new GameObject();

            var componentOnInstance = instance.AddComponent<TestComponent>();

            var child = GameObjectFactory.New()
                .SetParent(instance.transform, true)
                .AddComponent<TestComponent>()
                .Build();

            var component = Utilities.GameObject.GetComponentInDirectChildren<TestComponent>(instance, true);

            Assert.AreEqual(componentOnInstance, component);

            Utilities.Object.AutoDestroy(instance);
        }

        [Test]
        public void GetComponentInDirectChildrenShouldGetComponentInRootWithoutGenericTest()
        {
            var instance = new GameObject();

            var componentOnInstance = instance.AddComponent<TestComponent>();

            var child = GameObjectFactory.New()
                .SetParent(instance.transform, true)
                .AddComponent<TestComponent>()
                .Build();

            var component = Utilities.GameObject.GetComponentInDirectChildren<TestComponent>(instance, true);

            Assert.AreEqual(componentOnInstance, component);

            Utilities.Object.AutoDestroy(instance);
        }

        private class TestComponent : MonoBehaviour
        {
        }
    }
}