using System.Reflection;
using Omega.Tools.Experimental.Events;
using UnityEngine;

namespace Omega.Tools.Experimental.Event.Internals
{
    internal static class UnityHandlerAdapterBuilder
    {
        public static IEventHandler<TEvent> Build<TEvent>(IEventHandler<TEvent> handler, Object targetObject)
        {
            var invocationPolicy =
                handler.GetType().GetCustomAttribute<EventHandlerAttribute>()?.InvocationConvention ?? default;

            return new UnityHandlerAdapter<TEvent>(handler, invocationPolicy, targetObject);
        }
    }
}