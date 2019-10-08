using System;

namespace Omega.Tools.Experimental.Events
{
    public static class EventManager
    {
        public static void Event<TEvent>(TEvent arg)
            => EventManagerDispatcher<TEvent>.GetEventManager().Event(arg);

        public static void AddHandler<TEvent>(IEventHandler<TEvent> handler)
            => EventManagerDispatcher<TEvent>.GetEventManager().AddHandler(handler);

        public static void AddHandler<TEvent>(Action<TEvent> handler)
        {
            var actionHandlersProvider = EventManagerDispatcher<TEvent>.GetEventManagerActionInterface();
            if (actionHandlersProvider == null)
                throw new NotSupportedException(
                    $"Current EventManager of {typeof(TEvent)} not supported Action handlers." +
                    $"Implement interface {nameof(IActionHandlerInterface<TEvent>)} in EventManager to support Action handlers");
            
            actionHandlersProvider.AddHandler(handler);
        }

        public static void RemoveHandler<TEvent>(Action<TEvent> handler)
        {
            var actionHandlersProvider = EventManagerDispatcher<TEvent>.GetEventManagerActionInterface();
            if (actionHandlersProvider == null)
                throw new NotSupportedException(
                    $"Current EventManager of {typeof(TEvent)} not supported Action handlers." +
                    $"Implement interface {nameof(IActionHandlerInterface<TEvent>)} in EventManager to support Action handlers");
            
            actionHandlersProvider.RemoveHandler(handler);
        }

        public static void RemoveHandler<TEvent>(IEventHandler<TEvent> handler)
            => EventManagerDispatcher<TEvent>.GetEventManager().RemoveHandler(handler);
    }
}