using System;
using NUnit.Framework;
using Omega.Package;
using UnityEngine;

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
            Utilities.BoxCollider.SetAsBounds(boxCollider, bounds);

            Assert.AreEqual(bounds, boxCollider.bounds);
            
            Utilities.Object.AutoDestroy(gameObjectInstance);
        }

        [Test]
        public void SetAsBoundsShouldThrowArgumentNullExceptionWhenParameterIsNull()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => Utilities.BoxCollider.SetAsBounds(null, default));
        }

        [Test]
        public void SetAsBoundsShouldThrowMissingReferenceExceptionWhenParameterWereDestroyed()
        {
            var gameObjectInstance =
                new GameObject(nameof(SetAsBoundsShouldThrowMissingReferenceExceptionWhenParameterWereDestroyed));

            var collider = gameObjectInstance.AddComponent<BoxCollider>();

            Utilities.Object.AutoDestroy(gameObjectInstance);

            Assert.Throws<MissingReferenceException>(() =>
                Utilities.BoxCollider.SetAsBounds(collider, default));
        }
    }
}