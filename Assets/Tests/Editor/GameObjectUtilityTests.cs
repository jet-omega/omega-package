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

            var boxColliderByMissingComponent = Utilities.GameObject.MissingComponent<BoxCollider>(gameObjectInstance);
            var boxCollider = gameObjectInstance.GetComponent<BoxCollider>();

            Assert.AreEqual(boxCollider, boxColliderByMissingComponent);

            Utilities.Object.AutoDestroy(gameObjectInstance);        }

        [Test]
        public void MissingComponentShouldNotAddComponentWhenItIsOnGameObjectTest()
        {
            var gameObjectInstance =
                new GameObject(nameof(MissingComponentShouldNotAddComponentWhenItIsOnGameObjectTest));

            var boxCollider = gameObjectInstance.AddComponent<BoxCollider>();
            var boxColliderByMissingComponent = Utilities.GameObject.MissingComponent<BoxCollider>(gameObjectInstance);

            Assert.AreEqual(boxCollider, boxColliderByMissingComponent);

            Utilities.Object.AutoDestroy(gameObjectInstance);        }

        [Test]
        public void MissingComponentShouldThrowMissingReferenceExceptionWhenParameterIsDestroyedGameObjectTest()
        {
            var gameObjectInstance =
                new GameObject(nameof(MissingComponentShouldThrowMissingReferenceExceptionWhenParameterIsDestroyedGameObjectTest));

            var boxCollider = gameObjectInstance.AddComponent<BoxCollider>();

            Utilities.Object.AutoDestroy(gameObjectInstance);
            
            Assert.Throws<MissingReferenceException>(() =>
                Utilities.GameObject.MissingComponent<BoxCollider>(gameObjectInstance));
        }

        [Test]
        public void MissingComponentShouldThrowArgumentNullExceptionWhenParameterIsNullTest()
        {
            Assert.Throws<ArgumentNullException>(() =>
                // ReSharper disable once AssignNullToNotNullAttribute
                Utilities.GameObject.MissingComponent<BoxCollider>(null));
        }
    }
}