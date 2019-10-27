using System;
using System.Reflection;
using Omega.Experimental.Event.Attributes;
using Object = UnityEngine.Object;

namespace Omega.Experimental.Event.Internals
{
    internal static class ActionHandlerAdapterBuilder
    {
        public static IEventHandler<TEvent> Create<TEvent>(Action<TEvent> action)
        {
            if (action.Target is Object unityObject)
            {
                var invocationPolicy =
                    action.Method.GetCustomAttribute<EventHandlerAttribute>()?.InvocationPolicy ??
                    default;
                
                return new ActionHandlerUnityAdapter<TEvent>(action, unityObject, invocationPolicy);
            }

            return new ActionHandlerAdapter<TEvent>(action);
        }
    }
}