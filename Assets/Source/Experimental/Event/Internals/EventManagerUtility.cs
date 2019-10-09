using System;
using System.Linq;

namespace Omega.Tools.Experimental.Events.Internals
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