using System;
using System.Collections;
using Omega.Experimental.Event.Internals;
using Omega.Routines;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Omega.Experimental.Event
{
    public static class EventAggregator
    {
        public static void Event<TEvent>(TEvent arg)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new PlatformNotSupportedException();
#endif
            
            var handlers = EventManagerDispatcher<TEvent>.GetEventManager().GetEventHandlers();
            var runner = EventHandlerRunnerBuilder<TEvent>.Create(handlers, arg);
            EventScheduler.Schedule(runner);
        }
        

        public static Routine EventAsync<TEvent>(TEvent arg)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new PlatformNotSupportedException();
#endif
            
            var handlers = EventManagerDispatcher<TEvent>.GetEventManager().GetEventHandlers();
            var runner = EventHandlerRunnerBuilder<TEvent>.Create(handlers, arg);
            return EventScheduler.ExecuteAsync(runner);
        }


        public static void AddHandler<TEvent>(IEventHandler<TEvent> handler)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new PlatformNotSupportedException();
#endif
            
            if (handler is Object target)
                handler = UnityHandlerAdapterBuilder.Create(handler, target);
            
            EventManagerDispatcher<TEvent>.GetEventManager().AddHandler(handler);
        }

        public static void RemoveHandler<TEvent>(IEventHandler<TEvent> handler)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new PlatformNotSupportedException();
#endif
            if (handler is Object target)
                handler = UnityHandlerAdapterBuilder.Create(handler, target);
            
            EventManagerDispatcher<TEvent>.GetEventManager().RemoveHandler(handler);
        }

        public static void AddHandler<TEvent>(Action<TEvent> handler)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new PlatformNotSupportedException();
#endif
            var actionHandlerAdapter = ActionHandlerAdapterBuilder.Create(handler);
            EventManagerDispatcher<TEvent>.GetEventManager().AddHandler(actionHandlerAdapter);
        }

        public static void RemoveHandler<TEvent>(Action<TEvent> handler)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new PlatformNotSupportedException();
#endif
            var actionHandlerAdapter = ActionHandlerAdapterBuilder.Create(handler);
            EventManagerDispatcher<TEvent>.GetEventManager().RemoveHandler(actionHandlerAdapter);
        }
    }
}