using System;
using System.Collections.Generic;

namespace Omega.Tools.Experimental.Events
{
    public static class EventManager
    {
        private static Queue<IEvent> _queueProviders = new Queue<IEvent>();

        public static void Event<TEvent>(TEvent arg)
        {
            var handlersProvider = HandlersDispatcher<TEvent>.Provider;
            var handlersOfEvent = handlersProvider.GetHandlers();

            var @event = EventBuilder.CreateEvent(handlersOfEvent, arg);
            
            _queueProviders.Enqueue(@event);
            
            if(_queueProviders.Count == 1)
                EventMoveNext();
        }

        public static IEventHandler<TEvent> AddHandler<TEvent>(Action action)
        {
            var handler = HandlerBuilder.ByAction<TEvent>(action);
            AddHandler(handler);
            return handler;
        }
        
        public static void AddHandler<TEvent>(IEventHandler<TEvent> handler)
        {
            var handlersProvider = HandlersDispatcher<TEvent>.Provider;
            handlersProvider.AddHandler(handler);
        }

        public static void RemoveHandler<TEvent>(IEventHandler<TEvent> handler)
        {
            var handlersProvider = HandlersDispatcher<TEvent>.Provider;
            handlersProvider.RemoveHandler(handler);
        }

        private static void EventMoveNext()
        {
            while (_queueProviders.TryPeek(out var provider))
            {
                provider.Release();
                _queueProviders.Dequeue();
            }
        }
        
        //TODO: Remove or create queue extensions
        private static bool TryPeek<T>(this Queue<T> queue, out T value)
        {
            if(ReferenceEquals(queue, null))
                throw new NullReferenceException(nameof(queue));

            if (queue.Count == 0)
            {
                value = default;
                return false;
            }

            value = queue.Peek();
            return true;
        }
    }
}