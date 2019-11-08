using System;
using NUnit.Framework;
using Omega.Experimental;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Omega.Tools.Tests
{
    public sealed class TransformUtilityTests
    {
        [Test]
        public void GetChildsShouldReturnEmptyArrayFromSystemArrayEmpty()
        {
            var gameObjectInstance = new GameObject();

            var childs = Utilities.Transfrom.GetChilds(gameObjectInstance.transform);

            Assert.NotNull(childs);
            Assert.Zero(childs.Length);

            Assert.AreEqual(childs, Array.Empty<Transform>());
            
            Utilities.Object.AutoDestroy(gameObjectInstance);
        }

        [Test]
        public void GetChildsShouldThrowArgumentNullExceptionWhenParameterIsNull()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => Utilities.Transfrom.GetChilds(null));
        }

        [Test]
        public void GetChildsShouldThrowMissingReferenceExceptionWhenParameterWereDestroyed()
        {
            var gameObjectInstance = new GameObject();

            Utilities.Object.AutoDestroy(gameObjectInstance);

            Assert.Throws<MissingReferenceException>(() =>
                Utilities.Transfrom.GetChilds(gameObjectInstance.transform));
        }
    }
}