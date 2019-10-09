using System;
using System.Collections.Generic;
using System.Reflection;
using Omega.Tools.Experimental.Event;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Omega.Tools.Experimental.Events.Internals.EventManagers
{
    //TODO: Rework
    internal partial class UniversalEventManager<TEvent> : IEventManager<TEvent>, IEventManagerActionHandlerProvider<TEvent>
    {
        private Queue<IEvent> _queueEvents;
        private IHandlersProvider<TEvent> _handlerProvider;
        private Dictionary<Action<TEvent>, IEventHandler<TEvent>> _actionHandlers;

        public UniversalEventManager()
        {
            _queueEvents = new Queue<IEvent>();
            _handlerProvider = new DefaultHandlersProvider<TEvent>();
            _actionHandlers = new Dictionary<Action<TEvent>, IEventHandler<TEvent>>();
        }

        public void Event(TEvent arg)
        {
            var handlersOfEvent = _handlerProvider.GetHandlers();

            var @event = EventBuilder.CreateEventInternal(handlersOfEvent, arg);

            _queueEvents.Enqueue(@event);

            if (_queueEvents.Count == 1)
                EventMoveNext();
        }

        public void AddHandler(IEventHandler<TEvent> handler)
        {
            _handlerProvider.AddHandler(handler);
        }

        public void RemoveHandler(IEventHandler<TEvent> handler)
        {
            _handlerProvider.RemoveHandler(handler);
        }

        private void EventMoveNext()
        {
            while (_queueEvents.Count != 0)
            {
                var @event = _queueEvents.Peek();
                @event.Release();
                _queueEvents.Dequeue();
            }
        }

        public void AddHandler(Action<TEvent> action)
        {
            if (_actionHandlers.ContainsKey(action))
                throw new OverflowException(nameof(action));

            var handler = ActionHandlerBuilder.Build(action);
            _actionHandlers.Add(action, handler);

            AddHandler(handler);
        }

        public void RemoveHandler(Action<TEvent> action)
        {
            if (!_actionHandlers.TryGetValue(action, out var handler))
                return;

            RemoveHandler(handler);
            _actionHandlers.Remove(action);
        }
    }
}