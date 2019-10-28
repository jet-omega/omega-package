using NUnit.Framework;
using UnityEngine;
using UnityEditor.Build.Content;

namespace Omega.Tools.Tests.GameObjectFactories
{
    public class GameObjectFactoryNewTests
    {
        [Test]
        public void AddComponentShouldCreateGameObjectWithComponentTest()
        {
            var gameObject = GameObjectFactory.New().AddComponent<SomeComponent>().Build();
            Assert.True(GameObjectUtility.ContainsComponent<SomeComponent>(gameObject));
            Object.DestroyImmediate(gameObject);
        }

        [Test]
        public void RemoveComponentShouldRemoveFromFactoryComponentTest()
        {
            var gameObject = GameObjectFactory.New().AddComponent<SomeComponent>().RemoveComponent<SomeComponent>()
                .Build();
            Assert.False(GameObjectUtility.ContainsComponent<SomeComponent>(gameObject));
            Object.DestroyImmediate(gameObject);
        }

        [Test]
        public void SetNameShouldCreateGameObjectWithNameTest()
        {
            const string testName = "TestName";
            var gameObject = GameObjectFactory.New().SetName(testName).Build();
            Assert.AreEqual(testName, gameObject.name);
            Object.DestroyImmediate(gameObject);
        }

        [Test]
        public void SetLayerShouldCreateGameObjectWithLayerTest()
        {
            const int testLayer = 0x2;
            var gameObject = GameObjectFactory.New().SetLayer(testLayer).Build();
            Assert.AreEqual(testLayer, gameObject.layer);
            Object.DestroyImmediate(gameObject);
        }

        [Test]
        public void SetParentShouldRemoveFromFactoryComponentTest()
        {
            var newParent = new GameObject().transform;
            var gameObject = GameObjectFactory.New().SetParent(newParent, true).Build();
            Assert.AreEqual(newParent, gameObject.transform.parent);
            Object.DestroyImmediate(newParent.gameObject);
        }

        [Test]
        public void CustomShouldProcessCustomLogicTest()
        {
            var targetName = "key name for GameObject";
            var gameObject = GameObjectFactory.New()
                .Custom(go => go.name = targetName)
                .Build();

            Assert.AreEqual(targetName, gameObject.name);
            
            Object.DestroyImmediate(gameObject);
        }


        private class SomeComponent : MonoBehaviour
        {
        }
    }
}