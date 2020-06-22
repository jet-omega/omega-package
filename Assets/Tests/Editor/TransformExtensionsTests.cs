using NUnit.Framework;
using UnityEngine;

namespace Omega.Tools.Tests
{
    public class TransformExtensionsTests
    {
        [Test]
        public void ToRectTransformShouldGetRectTransformTest()
        {
            var transform = GameObjectFactory.New()
                .AddComponent<RectTransform>()
                .Build<Transform>();

            var castResult = transform.ToRectTransform(out var rectTransform);
            
            Assert.True(castResult);
            Assert.AreEqual(transform.transform, rectTransform);
        }
    }
}