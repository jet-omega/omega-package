using System;
using System.Reflection;
using Omega.Tools.Experimental.Events;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Omega.Tools.Experimental.Event.Internals
{
    internal static class ActionHandlerAdapterBuilder
    {
        public static IEventHandler<TEvent> Build<TEvent>(Action<TEvent> action)
        {
            if (action.Target is Object unityObject)
            {
                var invocationPolicy =
                    action.Method.GetCustomAttribute<EventHandlerAttribute>()?.InvocationConvention ??
                    default;
                
                return new ActionHandlerUnityAdapter<TEvent>(action, unityObject, invocationPolicy);
            }

            return new ActionHandlerAdapter<TEvent>(action);
        }
    }
}