using System;
using System.Collections;
using NUnit.Framework;
using Omega.Tools;
using UnityEngine;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace Omega.Tools.Tests
{
    public sealed class GameObjectUtilityTests
    {
        [Test]
        public void MissingComponentShouldAddComponentWhenItIsNotOnGameObjectTest()
        {
            var gameObjectInstance =
                new GameObject(nameof(MissingComponentShouldAddComponentWhenItIsNotOnGameObjectTest));

            var boxColliderByMissingComponent = GameObjectUtility.MissingComponent<BoxCollider>(gameObjectInstance);
            var boxCollider = gameObjectInstance.GetComponent<BoxCollider>();

            Assert.AreEqual(boxCollider, boxColliderByMissingComponent);

            Object.DestroyImmediate(gameObjectInstance);
        }

        [Test]
        public void MissingComponentShouldNotAddComponentWhenItIsOnGameObjectTest()
        {
            var gameObjectInstance =
                new GameObject(nameof(MissingComponentShouldNotAddComponentWhenItIsOnGameObjectTest));

            var boxCollider = gameObjectInstance.AddComponent<BoxCollider>();
            var boxColliderByMissingComponent = GameObjectUtility.MissingComponent<BoxCollider>(gameObjectInstance);

            Assert.AreEqual(boxCollider, boxColliderByMissingComponent);

            Object.DestroyImmediate(gameObjectInstance);
        }

        [Test]
        public void MissingComponentShouldThrowMissingReferenceExceptionWhenParameterIsDestroyedGameObjectTest()
        {
            var gameObjectInstance =
                new GameObject(nameof(MissingComponentShouldThrowMissingReferenceExceptionWhenParameterIsDestroyedGameObjectTest));

            var boxCollider = gameObjectInstance.AddComponent<BoxCollider>();

            Object.DestroyImmediate(gameObjectInstance);

            Assert.Throws<MissingReferenceException>(() =>
                GameObjectUtility.MissingComponent<BoxCollider>(gameObjectInstance));
        }

        [Test]
        public void MissingComponentShouldThrowArgumentNullExceptionWhenParameterIsNullTest()
        {
            Assert.Throws<ArgumentNullException>(() =>
                // ReSharper disable once AssignNullToNotNullAttribute
                GameObjectUtility.MissingComponent<BoxCollider>(null));
        }
    }
}