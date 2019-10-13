using System;
using System.Collections;
using Omega.Tools.Experimental.Event;
using Omega.Tools.Experimental.Events.Internals;
using Object = UnityEngine.Object;

namespace Omega.Tools.Experimental.Events
{
    public static class EventAggregator
    {
        public static void Event<TEvent>(TEvent arg)
            => EventManagerDispatcher<TEvent>.GetEventManager().Event(arg);

        public static IEnumerator EventAsync<TEvent>(TEvent arg)
            => EventManagerDispatcher<TEvent>.GetEventManager().EventAsync(arg);


        public static void AddHandler<TEvent>(IEventHandler<TEvent> handler)
        {
            if (handler is Object target)
                handler = new UnityHandlerAdapter<TEvent>(handler, target);
            
            EventManagerDispatcher<TEvent>.GetEventManager().AddHandler(handler);
        }

        public static void RemoveHandler<TEvent>(IEventHandler<TEvent> handler)
        {
            if (handler is Object target)
                handler = new UnityHandlerAdapter<TEvent>(handler, target);
            
            EventManagerDispatcher<TEvent>.GetEventManager().RemoveHandler(handler);
        }

        public static void AddHandler<TEvent>(Action<TEvent> handler)
        {
            var actionHandlerAdapter = ActionHandlerAdapterBuilder.Build(handler);
            EventManagerDispatcher<TEvent>.GetEventManager().AddHandler(actionHandlerAdapter);
        }

        public static void RemoveHandler<TEvent>(Action<TEvent> handler)
        {
            var actionHandlerAdapter = ActionHandlerAdapterBuilder.Build(handler);
            EventManagerDispatcher<TEvent>.GetEventManager().RemoveHandler(actionHandlerAdapter);
        }
    }
}