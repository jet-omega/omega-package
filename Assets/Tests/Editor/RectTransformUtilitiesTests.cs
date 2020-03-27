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
        public void GetChildsShouldReturnArrayWithTwoElementsWhenEightChildsIsRectAndTenChildsIsTransformTest()
        {
            //Test params
            const int countChildsWithRectTransform = 8;
            const int countChildsWithTransform = 10;

            var gameObjectInstance = GameObjectFactory.New().AddComponent<RectTransform>().Build();

            foreach (var num in Enumerable.Range(0, countChildsWithRectTransform))
                gameObjectInstance.Attach(new GameObject("Test " + num)).AddComponent<RectTransform>();

            foreach (var num in Enumerable.Range(countChildsWithRectTransform, countChildsWithTransform))
                gameObjectInstance.Attach(new GameObject("Test " + num));

            var childs = Utilities.RectTransform.GetChilds(gameObjectInstance.GetComponent<RectTransform>());

            Assert.AreEqual(childs.Length, countChildsWithRectTransform);

            Utilities.Object.AutoDestroy(gameObjectInstance);
        }

        [Test]
        public void GetChildsShouldReturnEmptyArrayWhenChildsWereDestroyedTest()
        {
            const int countChilds = 3;

            var gameObjectInstance = GameObjectFactory.New().AddComponent<RectTransform>().Build();

            foreach (var num in Enumerable.Range(0, countChilds))
                gameObjectInstance.Attach(new GameObject("Test " + num)).AddComponent<RectTransform>();

            Utilities.Transform.ClearChilds(gameObjectInstance.transform);

            var childs = Utilities.RectTransform.GetChilds(gameObjectInstance.GetComponent<RectTransform>());

            Assert.Zero(childs.Length);
        }

        [Test]
        public void GetChildsShouldThrowMissingReferenceExceptionWhenParameterIsDestroyedGameObjectTest()
        {
            var gameObjectInstance = GameObjectFactory.New().AddComponent<RectTransform>().Build();

            var rectTransform = gameObjectInstance.GetComponent<RectTransform>();

            Utilities.Object.AutoDestroy(gameObjectInstance);

            Assert.Throws<MissingReferenceException>(() => Utilities.RectTransform.GetChilds(rectTransform));
        }

        [Test]
        public void GetChildsShouldThrowArgumentNullExceptionWhenParameterIsNullTest()
        {
            Assert.Throws<ArgumentNullException>(() =>
                // ReSharper disable once AssignNullToNotNullAttribute
                Utilities.RectTransform.GetChilds(null));
        }
    }
}