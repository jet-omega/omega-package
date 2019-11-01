using NUnit.Framework;
using UnityEngine;

namespace Omega.Tools.Tests.GameObjectFactories
{
    public class GameObjectFactoryPrefabTests
    {
        [Test]
        public void PrefabShouldCreateGameObjectWithComponentTest()
        {
            var prefab = GameObjectFactory.New().AddComponent<SomeComponent>().SetName("Hello").Build();
            var gameObject = GameObjectFactory.Prefab(prefab).SetParent(prefab.transform, false).Build();

            Assert.True(GameObjectUtility.ContainsComponent<SomeComponent>(gameObject));

            Object.DestroyImmediate(prefab);
        }

        [Test]
        public void CustomShouldProcessCustomLogicTest()
        {
            var prefab = GameObjectFactory.New().AddComponent<SomeComponent>().SetName("Hello").Build();
            var targetName = "key name for GameObject";
            var gameObject = GameObjectFactory.Prefab(prefab)
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