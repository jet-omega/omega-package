using System;
using NUnit.Framework;

namespace Omega.Tools.Tests
{
    public class ReflectionUtilityTests
    {
        [Test]
        public void ContainsInterfaceDefinitionInTypeShouldReturnTrueWhenNotDefinedGenericTest()
        {
            var sourceType = typeof(ClassImplementInterfaceWithGeneric);
            var baseType = typeof(IBaseInterfaceWithGenericArg<>);

            var result = ReflectionUtility.ContainsInterfaceDefinitionInType(sourceType, baseType);

            Assert.True(result);
        }

        [Test]
        public void ContainsInterfaceDefinitionInTypeShouldReturnFalseTest()
        {
            var sourceType = typeof(ISimpleBaseInterface);
            var baseType = typeof(IBaseInterfaceWithGenericArg<>);

            var result = ReflectionUtility.ContainsInterfaceDefinitionInType(sourceType, baseType);

            Assert.False(result);
        }

        [Test]
        public void ContainsInterfaceDefinitionInTypeShouldReturnTrueWhenDefinedGenericInterfaceTest()
        {
            var sourceType = typeof(IBaseInterfaceWithGenericArg<object>);
            var baseType = typeof(IBaseBaseInterfaceWithGenericArg<>);

            var result = ReflectionUtility.ContainsInterfaceDefinitionInType(sourceType, baseType);

            Assert.True(result);
        }

        [Test]
        public void ContainsInterfaceDefinitionInTypeShouldReturnTrueWhenNotDefinedGenericInterfaceTest()
        {
            var sourceType = typeof(IBaseInterfaceWithGenericArg<>);
            var baseType = typeof(IBaseBaseInterfaceWithGenericArg<>);

            var result = ReflectionUtility.ContainsInterfaceDefinitionInType(sourceType, baseType);

            Assert.True(result);
        }

        [Test]
        public void ContainsInterfaceDefinitionInTypeShouldThrowArgumentExceptionWhenSecondArgumentIsNotDefinition()
        {
            var sourceType = typeof(ClassImplementInterfaceWithGeneric);
            var baseType = typeof(IBaseInterfaceWithGenericArg<object>);

            Assert.Throws<ArgumentException>(
                () => ReflectionUtility.ContainsInterfaceDefinitionInType(sourceType, baseType));
        }

        [Test]
        public void GetGenericArgumentsOfDefinitionInterfaceShouldReturnCorrectGenericTypeTest()
        {
            var sourceType = typeof(ClassImplementInterfaceWithGeneric);
            var definitionBaseInterface = typeof(IBaseInterfaceWithGenericArg<>);

            var genericTypes =
                ReflectionUtility.GetGenericArgumentsOfDefinitionInterface(sourceType, definitionBaseInterface);

            Assert.AreEqual(1, genericTypes.Length);
            Assert.Contains(typeof(object), genericTypes);
        }

        [Test]
        public void GetGenericArgumentsOfDefinitionInterfaceShouldThrowArgumentException()
        {
            var sourceType = typeof(ClassImplementInterfaceWithGeneric);
            var definitionBaseInterface = typeof(ISingle<>);

            Assert.Throws<ArgumentException>(
                () => ReflectionUtility.GetGenericArgumentsOfDefinitionInterface(sourceType, definitionBaseInterface));
        }

        private interface ISimpleBaseInterface
        {
        }

        private sealed class SimpleClass : ISimpleBaseInterface
        {
        }

        private interface ISingle<T>
        {
        }

        private interface IBaseBaseInterfaceWithGenericArg<T>
        {
        }

        private interface IBaseInterfaceWithGenericArg<T> : IBaseBaseInterfaceWithGenericArg<T>
        {
        }

        private sealed class ClassImplementInterfaceWithGeneric : IBaseInterfaceWithGenericArg<object>
        {
        }
    }
}