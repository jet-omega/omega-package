using System;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Omega.Tools.Tests
{
    public class RectTransformUtilityTests
    {
        [Test]
        public void GetChildsShouldReturnArrayWithTwoElementsWhenEightChildsIsRectAndTenChildsIsTransformTest()
        {
            //Test params
            const int countChildsWithRectTransform = 8;
            const int countChildsWithTransform = 10;

            var gameObjectInstance = new GameObject("GetChildsShouldReturnArrayWithTwoElementsTest");
            gameObjectInstance.AddComponent<RectTransform>();

            foreach (var num in Enumerable.Range(0, countChildsWithRectTransform))
                gameObjectInstance.Attach(new GameObject("Test " + num)).AddComponent<RectTransform>();

            foreach (var num in Enumerable.Range(countChildsWithRectTransform, countChildsWithTransform))
                gameObjectInstance.Attach(new GameObject("Test " + num));

            var childs = RectTransformUtility.GetChilds(gameObjectInstance.GetComponent<RectTransform>());

            Assert.AreEqual(childs.Length, countChildsWithRectTransform);

            Object.DestroyImmediate(gameObjectInstance);
        }

        [Test]
        public void GetChildsShouldReturnEmptyArrayWhenChildsWereDestroyedTest()
        {
            const int countChilds = 3;

            var gameObjectInstance =
                new GameObject(nameof(GetChildsShouldReturnEmptyArrayWhenChildsWereDestroyedTest));

            gameObjectInstance.AddComponent<RectTransform>();

            foreach (var num in Enumerable.Range(0, countChilds))
                gameObjectInstance.Attach(new GameObject("Test " + num)).AddComponent<RectTransform>();

            gameObjectInstance.GetChilds().Select(e => e.gameObject).ToList().ForEach(Object.DestroyImmediate);

            var childs = RectTransformUtility.GetChilds(gameObjectInstance.GetComponent<RectTransform>());

            Assert.Zero(childs.Length);
        }

        [Test]
        public void GetChildsShouldThrowMissingReferenceExceptionWhenParameterIsDestroyedGameObjectTest()
        {
            var gameObjectInstance =
                new GameObject(
                    nameof(GetChildsShouldThrowMissingReferenceExceptionWhenParameterIsDestroyedGameObjectTest));

            var rectTransform = gameObjectInstance.AddComponent<RectTransform>();

            Object.DestroyImmediate(gameObjectInstance);

            Assert.Throws<MissingReferenceException>(() => RectTransformUtility.GetChilds(rectTransform));
        }
        
        [Test]
        public void GetChildsShouldThrowArgumentNullExceptionWhenParameterIsNullTest()
        {
            Assert.Throws<ArgumentNullException>(() =>
                // ReSharper disable once AssignNullToNotNullAttribute
                RectTransformUtility.GetChilds(null));
        }
    }
}