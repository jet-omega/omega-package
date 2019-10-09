using System;
using NUnit.Framework;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Omega.Tools.Tests
{
    public sealed class TransformUtilityTests
    {
        [Test]
        public void GetChildesShouldReturnEmptyArrayFromSystemArrayEmpty()
        {
            var gameObjectInstance = new GameObject(nameof(GetChildesShouldReturnEmptyArrayFromSystemArrayEmpty));

            var childes = TransformUtility.GetChildes(gameObjectInstance.transform);

            Assert.NotNull(childes);
            Assert.Zero(childes.Length);

            Assert.AreEqual(childes, Array.Empty<Transform>());
            
            Object.DestroyImmediate(gameObjectInstance);
        }

        [Test]
        public void GetChildesShouldThrowArgumentNullExceptionWhenParameterIsNull()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => TransformUtility.GetChildes(null));
        }

        [Test]
        public void GetChildesShouldThrowMissingReferenceExceptionWhenParameterWereDestroyed()
        {
            var gameObjectInstance =
                new GameObject(nameof(GetChildesShouldThrowMissingReferenceExceptionWhenParameterWereDestroyed));

            Object.DestroyImmediate(gameObjectInstance);

            Assert.Throws<MissingReferenceException>(() =>
                TransformUtility.GetChildes(gameObjectInstance.transform));
        }
    }
}