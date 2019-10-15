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

        private class SomeComponent : MonoBehaviour
        {
        }
    }
}