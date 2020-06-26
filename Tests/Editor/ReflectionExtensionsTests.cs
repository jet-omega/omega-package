using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Omega.Package;
using UnityEngine;

namespace Omega.Tools.Tests
{
    public class ReflectionExtensionsTests
    {
        private const BindingFlags GetMemberFlags = BindingFlags.NonPublic | BindingFlags.Instance;

        [Test]
        public void GetReturnTypeTest()
        {
            var classType = typeof(GetReturnTypeTestClass);

            var field = classType.GetMember("Field", GetMemberFlags).First();
            var property = classType.GetMember("Property", GetMemberFlags).First();
            var method = classType.GetMember("Method", GetMemberFlags).First();
            var @event = classType.GetMember("Event", GetMemberFlags).First();

            var fieldReturnType = field.GetReturnType();
            var propertyReturnType = property.GetReturnType();
            var methodReturnType = method.GetReturnType();
            var eventReturnType = @event.GetReturnType();
            
            Assert.AreEqual(typeof(int), fieldReturnType);
            Assert.AreEqual(typeof(int), propertyReturnType);
            Assert.AreEqual(typeof(int), methodReturnType);
            Assert.AreEqual(typeof(EventHandler<int>), eventReturnType);
        }

        [Test]
        public void GetMemberValueTest()
        {
            var @object = new GetReturnTypeTestClass();
            var classType = typeof(GetReturnTypeTestClass);

            var field = classType.GetMember("Field", GetMemberFlags).First();
            var property = classType.GetMember("Property", GetMemberFlags).First();
            
            Assert.AreEqual(1, field.GetMemberValue(@object));
            Assert.AreEqual(1, property.GetMemberValue(@object));
        }
        
        [Test]
        public void SetMemberValueTest()
        {
            var @object = new GetReturnTypeTestClass();
            var classType = typeof(GetReturnTypeTestClass);

            var field = classType.GetMember("Field", GetMemberFlags).First();
            var property = classType.GetMember("Property",GetMemberFlags).First();

            field.SetMemberValue(@object, 2);
            property.SetMemberValue(@object, 2);
            
            Assert.AreEqual(2, field.GetMemberValue(@object));
            Assert.AreEqual(2, property.GetMemberValue(@object));
        }

        class GetReturnTypeTestClass
        {
            private int Field = 1;
            private int Property { get; set; } = 1;
            private int Method() => 1;
            private event EventHandler<int> Event;

            private delegate int EventHandler();
        }
    }
}
