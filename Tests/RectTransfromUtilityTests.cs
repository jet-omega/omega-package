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
        public void GetChildesShouldReturnArrayWithTwoElementsWhenEightChildesIsRectAndTenChildesIsTransformTest()
        {
            //Test params
            const int countChildesWithRectTransform = 8;
            const int countChildesWithTransform = 10;

            var gameObjectInstance = new GameObject("GetChildesShouldReturnArrayWithTwoElementsTest");
            gameObjectInstance.AddComponent<RectTransform>();

            foreach (var num in Enumerable.Range(0, countChildesWithRectTransform))
                gameObjectInstance.Attach(new GameObject("Test " + num)).AddComponent<RectTransform>();

            foreach (var num in Enumerable.Range(countChildesWithRectTransform, countChildesWithTransform))
                gameObjectInstance.Attach(new GameObject("Test " + num));

            var childes = RectTransformUtility.GetChildes(gameObjectInstance.GetComponent<RectTransform>());

            Assert.AreEqual(childes.Length, countChildesWithRectTransform);

            Object.Destroy(gameObjectInstance);
        }

        [Test]
        public void GetChildesShouldReturnEmptyArrayWhenChildesWereDestroyedTest()
        {
            const int countChildes = 3;

            var gameObjectInstance =
                new GameObject(nameof(GetChildesShouldReturnEmptyArrayWhenChildesWereDestroyedTest));

            gameObjectInstance.AddComponent<RectTransform>();

            foreach (var num in Enumerable.Range(0, countChildes))
                gameObjectInstance.Attach(new GameObject("Test " + num)).AddComponent<RectTransform>();

            gameObjectInstance.GetChildes().Select(e => e.gameObject).ToList().ForEach(Object.DestroyImmediate);

            var childes = RectTransformUtility.GetChildes(gameObjectInstance.GetComponent<RectTransform>());

            Assert.Zero(childes.Length);
        }

        [Test]
        public void GetChildesShouldThrowMissingReferenceExceptionWhenParameterIsDestroyedGameObjectTest()
        {
            var gameObjectInstance =
                new GameObject(
                    nameof(GetChildesShouldThrowMissingReferenceExceptionWhenParameterIsDestroyedGameObjectTest));

            var rectTransform = gameObjectInstance.AddComponent<RectTransform>();

            Object.DestroyImmediate(gameObjectInstance);

            Assert.Throws<MissingReferenceException>(() => RectTransformUtility.GetChildes(rectTransform));
        }
        
        [Test]
        public void GetChildesShouldThrowArgumentNullExceptionWhenParameterIsNullTest()
        {
            Assert.Throws<ArgumentNullException>(() =>
                // ReSharper disable once AssignNullToNotNullAttribute
                RectTransformUtility.GetChildes(null));
        }
    }
}