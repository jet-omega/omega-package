using NUnit.Framework;
using Omega.Package;
using UnityEngine;

namespace Omega.Tools.Tests.GameObjectFactories
{
    public class GameObjectFactoryPrefabTests
    {
        [Test]
        public void PrefabShouldCreateGameObjectWithComponentTest()
        {
            var prefab = GameObjectFactory.New().AddComponent<SomeComponent>().Build();
            var gameObject = GameObjectFactory.Prefab(prefab).SetParent(prefab.transform, false).Build();

            Assert.True(Utilities.GameObject.ContainsComponent<SomeComponent>(gameObject));

            Assert.False(!prefab);
            Assert.False(!gameObject);
            
            Utilities.Object.AutoDestroy(gameObject, prefab);
        }

        [Test]
        public void CustomShouldProcessCustomLogicTest()
        {
            var prefab = GameObjectFactory.New().AddComponent<SomeComponent>().Build();
            var targetName = "key name for GameObject";
            var gameObject = GameObjectFactory.Prefab(prefab)
                .Custom(go => go.name = targetName)
                .Build();

            Assert.AreEqual(targetName, gameObject.name);

            Utilities.Object.AutoDestroy(gameObject, prefab);
        }

        private class SomeComponent : MonoBehaviour
        {
        }
    }
}