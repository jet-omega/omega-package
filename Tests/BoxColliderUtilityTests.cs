using System;
using NUnit.Framework;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Omega.Tools.Tests
{
    public class BoxColliderUtilityTests
    {
        [Test]
        public void SetAsBoundsSetupCorrectBounds()
        {
            var bounds = new Bounds(Vector3.one * -5, Vector3.one * 10);

            var gameObjectInstance = new GameObject(nameof(SetAsBoundsSetupCorrectBounds));
            var boxCollider = gameObjectInstance.AddComponent<BoxCollider>();
            BoxColliderUtility.SetAsBounds(boxCollider, bounds);

            Assert.AreEqual(bounds, boxCollider.bounds);
            
            Object.Destroy(gameObjectInstance);
        }

        [Test]
        public void SetAsBoundsShouldThrowArgumentNullExceptionWhenParameterIsNull()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => BoxColliderUtility.SetAsBounds(null, default));
        }

        [Test]
        public void SetAsBoundsShouldThrowMissingReferenceExceptionWhenParameterWereDestroyed()
        {
            var gameObjectInstance =
                new GameObject(nameof(SetAsBoundsShouldThrowMissingReferenceExceptionWhenParameterWereDestroyed));

            var collider = gameObjectInstance.AddComponent<BoxCollider>();

            Object.DestroyImmediate(gameObjectInstance);

            Assert.Throws<MissingReferenceException>(() =>
                BoxColliderUtility.SetAsBounds(collider, default));
        }
    }
}