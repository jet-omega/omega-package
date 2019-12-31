using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Omega.Package.Tests.Editor
{
    public class TypeHelperTests
    {
        [Test]
        public void ImplicitOperatorTypeHelperToTypeTest()
        {
            Type type = TypeHelper.For<float>();
            Assert.AreEqual(typeof(float), type);
        }

        [Test]
        public void ForGenericTypeShouldGetTypeHelperForTypeOfGenericTest()
        {
            Type listOfFloatType = TypeHelper.For(typeof(List<>)).ForGenericType<float>();
            Assert.AreEqual(typeof(List<float>), listOfFloatType);
        }
    }
}