using System;
using System.Collections.Generic;
using System.Reflection;
using Omega.Experimental.Event.Attributes;
using UnityEngine;

namespace Omega.Experimental.Event.Internals
{
    internal static class EventHandlerRunnerBuilder<TEvent>
    {
        public static IEvent Create(IEnumerable<IEventHandler<TEvent>> handlers, TEvent eventArg)
        {
            var eventType = typeof(TEvent);
            var eventHandlingAttribute = eventType.GetCustomAttribute<EventHandlingAttribute>();

            if (eventHandlingAttribute == null || eventHandlingAttribute.Handling == EventHandling.NotIsolate)
                return new NotIsolateEventHandlerRunner<TEvent>(handlers, eventArg);
            if (eventHandlingAttribute.Handling == EventHandling.IsolateEvent)
                return new IsolateEventEventHandlerRunner<TEvent>(handlers, eventArg, Debug.LogException);
            if (eventHandlingAttribute.Handling == EventHandling.IsolateHandlers)
                return new IsolateHandlersEventHandlerRunner<TEvent>(handlers, eventArg,
                    (h, e) => Debug.LogException(e));

            throw new ArgumentOutOfRangeException(nameof(eventHandlingAttribute.Handling),
                eventHandlingAttribute.Handling, "Handling field value in attribute had an invalid value");
        }
    }
}