using System;
using NUnit.Framework;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Omega.Tools.Tests
{
    public sealed class TransformUtilityTests
    {
        [Test]
        public void GetChildsShouldReturnEmptyArrayFromSystemArrayEmpty()
        {
            var gameObjectInstance = new GameObject(nameof(GetChildsShouldReturnEmptyArrayFromSystemArrayEmpty));

            var childs = TransformUtility.GetChilds(gameObjectInstance.transform);

            Assert.NotNull(childs);
            Assert.Zero(childs.Length);

            Assert.AreEqual(childs, Array.Empty<Transform>());
            
            Object.DestroyImmediate(gameObjectInstance);
        }

        [Test]
        public void GetChildsShouldThrowArgumentNullExceptionWhenParameterIsNull()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => TransformUtility.GetChilds(null));
        }

        [Test]
        public void GetChildsShouldThrowMissingReferenceExceptionWhenParameterWereDestroyed()
        {
            var gameObjectInstance =
                new GameObject(nameof(GetChildsShouldThrowMissingReferenceExceptionWhenParameterWereDestroyed));

            Object.DestroyImmediate(gameObjectInstance);

            Assert.Throws<MissingReferenceException>(() =>
                TransformUtility.GetChilds(gameObjectInstance.transform));
        }
    }
}