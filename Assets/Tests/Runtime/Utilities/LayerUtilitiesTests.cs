using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace Omega.Package.Tests
{
    public class LayerUtilitiesTests
    {
        [Test]
        public void LayersInMaskTest()
        {
            var testMask = LayerMask.GetMask("Default", "Water", "PostProcessing");
            var expected = new List<int>
                {LayerMask.NameToLayer("Default"), LayerMask.NameToLayer("Water"), LayerMask.NameToLayer("PostProcessing")};
            var actual = new List<int>();
            Utilities.Layer.LayersInMask(testMask, actual);
            Assert.AreEqual(expected, actual);
        }
    }
}