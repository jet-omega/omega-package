using System;
using System.Reflection;
using Omega.Tools.Experimental.Event;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Omega.Tools.Experimental.Events.Internals.EventManagers
{
    internal partial class UniversalEventManager<TEvent>
    {
        private static class ActionHandlerBuilder
        {
            public static IEventHandler<TEvent> Build(Action<TEvent> action)
            {
                if (action.Target is Object)
                {
                    var method = action.Method;

                    var eventHandlerAttribute = method.GetCustomAttribute<EventHandlerAttribute>();

                    return new UnityActionHandler(action,
                        eventHandlerAttribute?.InvocationConvention ??
                        InvocationConvention.PreventInvocationFromDestroyedObject);
                }

                return new ActionHandler(action);
            }
        } 
        
        private sealed class UnityActionHandler : IEventHandler<TEvent>
        {
            private readonly Action<TEvent> _action;
            private readonly InvocationConvention _invocationConvention;

            public UnityActionHandler(Action<TEvent> action, InvocationConvention invocationConvention)
            {
                _action = action ?? throw new ArgumentNullException(nameof(action));
                _invocationConvention = invocationConvention;
            }

            public void Execute(TEvent arg)
            {
                if (_invocationConvention == InvocationConvention.AllowInvocationFromDestroyedObject)
                    _action(arg);
                else
                {
                    if ((bool) (Object) _action.Target)
                        _action(arg);
                    else
                    {
                        if (_invocationConvention ==
                            InvocationConvention.AllowInvocationFromDestroyedObjectButLogWarning)
                            Debug.LogWarning(
                                $"Invocation handler (method: {_action.Method.Name}) from destroyed object (type:{_action.Target.GetType()})");
                        else
                            throw new Exception();
                    }
                }
            }
        }
        private sealed class ActionHandler : IEventHandler<TEvent>
        {
            private readonly Action<TEvent> _action;

            public ActionHandler(Action<TEvent> action)
                => _action = action ?? throw new ArgumentNullException(nameof(action));

            public void Execute(TEvent arg)
            {
                _action(arg);
            }
        }
    }
}