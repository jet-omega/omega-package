using System;
using System.Linq;
using NUnit.Framework;
using Omega.Package;
using UnityEngine;

namespace Omega.Tools.Tests
{
    public class RectTransformUtilitiesTests
    {
        [Test]
        public void GetChildrenShouldReturnArrayWithTwoElementsWhenEightChildrenIsRectAndTenChildrenIsTransformTest()
        {
            //Test params
            const int countChildrenWithRectTransform = 8;
            const int countChildrenWithTransform = 10;

            var gameObjectInstance = GameObjectFactory.New().AddComponent<RectTransform>().Build();

            foreach (var num in Enumerable.Range(0, countChildrenWithRectTransform))
            {
                var newChild = new GameObject("Test " + num).AddComponent<RectTransform>();
                newChild.transform.SetParent(gameObjectInstance.transform);
            }

            foreach (var num in Enumerable.Range(countChildrenWithRectTransform, countChildrenWithTransform))
            {
                var newChild = new GameObject("Test " + num);
                newChild.transform.SetParent(gameObjectInstance.transform);
            }

            var children = Utilities.RectTransform.GetChildren(gameObjectInstance.GetComponent<RectTransform>());

            Assert.AreEqual(children.Length, countChildrenWithRectTransform);

            Utilities.Object.AutoDestroy(gameObjectInstance);
        }

        [Test]
        public void GetChildrenShouldReturnEmptyArrayWhenChildrenWereDestroyedTest()
        {
            const int countChildren = 3;

            var gameObjectInstance = GameObjectFactory.New().AddComponent<RectTransform>().Build();

            foreach (var num in Enumerable.Range(0, countChildren))
            {
                var newChild = new GameObject("Test " + num).AddComponent<RectTransform>();
                newChild.SetParent(gameObjectInstance.transform);
            }

            Utilities.Transform.DestroyChildren(gameObjectInstance.transform);

            var children = Utilities.RectTransform.GetChildren(gameObjectInstance.GetComponent<RectTransform>());

            Assert.Zero(children.Length);
        }

        [Test]
        public void GetChildrenShouldThrowMissingReferenceExceptionWhenParameterIsDestroyedGameObjectTest()
        {
            var gameObjectInstance = GameObjectFactory.New().AddComponent<RectTransform>().Build();

            var rectTransform = gameObjectInstance.GetComponent<RectTransform>();

            Utilities.Object.AutoDestroy(gameObjectInstance);

            Assert.Throws<MissingReferenceException>(() => Utilities.RectTransform.GetChildren(rectTransform));
        }

        [Test]
        public void GetChildrenShouldThrowArgumentNullExceptionWhenParameterIsNullTest()
        {
            Assert.Throws<ArgumentNullException>(() =>
                // ReSharper disable once AssignNullToNotNullAttribute
                Utilities.RectTransform.GetChildren(null));
        }
    }
}