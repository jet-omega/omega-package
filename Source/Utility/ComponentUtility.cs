using System;
using UnityEngine;

namespace Omega.Tools
{
    public static class ComponentUtility
    {
        public static bool TryInvoke(Component component, string methodName)
        {
            if (ReferenceEquals(component, null))
                throw new ArgumentNullException(nameof(component));
            if (!component)
                throw new MissingReferenceException(nameof(component));

            if (string.IsNullOrEmpty(methodName))
                throw new ArgumentException(nameof(methodName));

            return TryInvokeWithoutChecks(component, methodName);
        }

        public static bool TryInvoke(Component component, string methodName, object arg)
        {
            if (ReferenceEquals(component, null))
                throw new ArgumentNullException(nameof(component));
            if (!component)
                throw new MissingReferenceException(nameof(component));

            if (string.IsNullOrEmpty(methodName))
                throw new ArgumentException(nameof(methodName));

            return TryInvokeWithoutChecks(component, methodName, arg);
        }

        internal static bool TryInvokeWithoutChecks(Component component, string methodName)
        {
            var componentType = component.GetType();
            var method = componentType.GetMethod(methodName, Array.Empty<Type>());
            if (method == null)
                return false;

            method.Invoke(component, Array.Empty<object>());
            return true;
        }

        internal static bool TryInvokeWithoutChecks(Component component, string methodName, object arg)
        {
            var componentType = component.GetType();
            var method = componentType.GetMethod(methodName, new[] {arg.GetType()});
            if (method == null)
                return false;

            method.Invoke(component, new[] {arg});
            return true;
        }
    }
}