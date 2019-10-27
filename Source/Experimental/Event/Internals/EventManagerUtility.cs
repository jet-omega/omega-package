using System;
using System.Linq;
using Omega.Tools;

namespace Omega.Experimental.Event.Internals
{
    internal static class EventManagerUtility
    {
        internal static Type GetEventTypeOfEventManagerType(Type eventManagerType)
        {
            var genericArguments = ReflectionUtility.GetGenericArgumentsOfDefinitionInterface(eventManagerType,
                typeof(IEventManager<>));

            return genericArguments.First();
        }

        internal static bool IsEventManagerType(Type eventManager)
        {
            return ReflectionUtility.ContainsInterfaceDefinitionInType(eventManager, typeof(IEventManager<>));
        }
    }
}