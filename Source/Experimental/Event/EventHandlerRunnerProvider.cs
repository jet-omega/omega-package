using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Omega.Tools.Experimental.Events;
using UnityEngine;

namespace Omega.Tools.Experimental.Event
{
    internal static class EventHandlerRunnerProvider<TEvent>
    {
        public static IEvent CreateRunner(IEnumerable<IEventHandler<TEvent>> handlers, TEvent eventArg)
        {
            var eventType = typeof(TEvent);
            var eventHandlingAttribute = eventType.GetCustomAttribute<EventHandlingAttribute>();

            if (eventHandlingAttribute == null || eventHandlingAttribute.Handling == EventHandling.NotIsolate)
                return new SafeEventHandlerRunner<TEvent>(handlers, eventArg);
            if (eventHandlingAttribute.Handling == EventHandling.IsolateEvent)
                return new HierarchyEventHandlerRunner<TEvent>(handlers, eventArg, Debug.LogException);
            if (eventHandlingAttribute.Handling == EventHandling.IsolateHandlers)
                return new AggressiveEventHandlerRunner<TEvent>(handlers, eventArg, (h, e) => Debug.LogException(e));

            throw new Exception();
        }
    }
}