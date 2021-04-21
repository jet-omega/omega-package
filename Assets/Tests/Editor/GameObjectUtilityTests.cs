using System;
using NUnit.Framework;
using Omega.Package;
using UnityEngine;

namespace Omega.Tools.Tests
{
    public sealed class GameObjectUtilitiesTests
    {
        [Test]
        public void MissingComponentShouldAddComponentWhenItIsNotOnGameObjectTest()
        {
            var gameObjectInstance =
                new GameObject(nameof(MissingComponentShouldAddComponentWhenItIsNotOnGameObjectTest));

            var boxColliderByMissingComponent = gameObjectInstance.MissingComponent<BoxCollider>();
            var boxCollider = gameObjectInstance.GetComponent<BoxCollider>();

            Assert.AreEqual(boxCollider, boxColliderByMissingComponent);

            Utilities.Object.AutoDestroy(gameObjectInstance);
        }

        [Test]
        public void MissingComponentShouldNotAddComponentWhenItIsOnGameObjectTest()
        {
            var gameObjectInstance =
                new GameObject(nameof(MissingComponentShouldNotAddComponentWhenItIsOnGameObjectTest));

            var boxCollider = gameObjectInstance.AddComponent<BoxCollider>();
            var boxColliderByMissingComponent = gameObjectInstance.MissingComponent<BoxCollider>();

            Assert.AreEqual(boxCollider, boxColliderByMissingComponent);

            Utilities.Object.AutoDestroy(gameObjectInstance);
        }

        [Test]
        public void MissingComponentShouldThrowMissingReferenceExceptionWhenParameterIsDestroyedGameObjectTest()
        {
            var gameObjectInstance =
                new GameObject(
                    nameof(MissingComponentShouldThrowMissingReferenceExceptionWhenParameterIsDestroyedGameObjectTest));

            var boxCollider = gameObjectInstance.AddComponent<BoxCollider>();

            Utilities.Object.AutoDestroy(gameObjectInstance);

            Assert.Throws<MissingReferenceException>(() =>
                gameObjectInstance.MissingComponent<BoxCollider>());
        }
    }
}