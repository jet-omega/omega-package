using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace Omega.Tools
{
    public static class ReflectionUtility
    {
        public static MethodInfo[] GetMethodsWithAttribute<T>([NotNull] Type type)
            where T : Attribute
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return GetMethodsWithAttributeWithoutChecks<T>(type, null).ToArray();
        }

        public static MethodInfo[] GetMethodsWithAttribute<T>([NotNull] Type type, [CanBeNull] Func<T, bool> selector)
            where T : Attribute
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return GetMethodsWithAttributeWithoutChecks<T>(type, selector).ToArray();
        }

        public static bool ContainsInterfaceDefinitionInType(Type type, Type definitionType)
        {
            if(type == null)
                throw new ArgumentNullException(nameof(type));
            if(definitionType == null)
                throw new ArgumentNullException(nameof(definitionType));
            if(!definitionType.IsGenericTypeDefinition)
                throw new ArgumentException(nameof(definitionType));

            return ContainsInterfaceDefinitionInTypeWithoutChecks(type, definitionType);
        }

        public static Type[] GetGenericArgumentsOfDefinitionInterface(Type sourceType, Type definitionInterface)
        {
            if(sourceType == null)
                throw new ArgumentNullException(nameof(sourceType));
            if(definitionInterface == null)
                throw new ArgumentNullException(nameof(definitionInterface));
            
            if(!definitionInterface.IsInterface)
                throw new ArgumentException(nameof(definitionInterface));
            if(!definitionInterface.IsGenericTypeDefinition)
                throw new ArgumentException(nameof(definitionInterface));

            if (!ContainsInterfaceDefinitionInTypeWithoutChecks(sourceType, definitionInterface))
                throw new ArgumentException();

            return GetGenericArgumentsOfDefinitionInterfaceWithoutChecks(sourceType, definitionInterface);
        }
        
        internal static Type[] GetGenericArgumentsOfDefinitionInterfaceWithoutChecks(Type sourceType, Type definitionInterface)
        {
            return sourceType.GetInterfaces()
                .Where(_ => _.IsGenericType)
                .First(_ => _.GetGenericTypeDefinition() == definitionInterface)
                .GetGenericArguments();
        }
        
        internal static bool ContainsInterfaceDefinitionInTypeWithoutChecks(Type type, Type definitionType)
        {
            return type.GetInterfaces()
                    .Where(_ => _.IsGenericType)
                    .Select(_ => _.GetGenericTypeDefinition())
                    .Any(_ => definitionType == _);
        }

        [NotNull]
        internal static IEnumerable<MethodInfo> GetMethodsWithAttributeWithoutChecks<T>([NotNull] Type type,
            [CanBeNull] Func<T, bool> selector)
            where T : Attribute
        {
            var attributeType = typeof(T);
            var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var methodsWithAttribute = methods.Where(_ => _.GetCustomAttribute(attributeType) != null);

            if (selector != null)
                methodsWithAttribute = methodsWithAttribute.Where(_ => selector(_.GetCustomAttribute<T>()));

            return methodsWithAttribute;
        }
    }
}