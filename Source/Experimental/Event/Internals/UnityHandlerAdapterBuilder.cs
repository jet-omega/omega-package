using System.Reflection;
using Omega.Tools.Experimental.Events;
using UnityEngine;

namespace Omega.Tools.Experimental.Event.Internals
{
    internal static class UnityHandlerAdapterBuilder
    {
        public static IEventHandler<TEvent> Create<TEvent>(IEventHandler<TEvent> handler, Object targetObject)
        {
            var invocationPolicy =
                handler.GetType().GetCustomAttribute<EventHandlerAttribute>()?.InvocationPolicy ?? default;

            return new UnityHandlerAdapter<TEvent>(handler, invocationPolicy, targetObject);
        }
    }
}