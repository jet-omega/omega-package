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
            if(type == null)
                throw new ArgumentNullException(nameof(type));

            return GetMethodsWithAttributeWithoutChecks<T>(type, null).ToArray();
        }
        
        public static MethodInfo[] GetMethodsWithAttribute<T>([NotNull] Type type, [CanBeNull] Func<T, bool> selector)
            where T : Attribute
        {
            if(type == null)
                throw new ArgumentNullException(nameof(type));

            return GetMethodsWithAttributeWithoutChecks<T>(type, selector).ToArray();
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