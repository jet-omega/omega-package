using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using NUnit.Framework;
using UnityEngine.Scripting;

namespace Omega.Package.Tests.Editor
{
    [Preserve]
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

        [Test]
        public void GetAttributeShouldReturnAttributeInstanceTest()
        {
            Assert.NotNull(TypeHelper.For<TypeHelperTests>().GetAttribute<PreserveAttribute>());
            // Assert.NotNull(typeof(TypeHelperTests).GetCustomAttribute<PreserveAttribute>());
        }
    }
}