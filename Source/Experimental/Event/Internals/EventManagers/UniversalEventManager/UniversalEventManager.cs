using System;
using System.Collections.Generic;
using System.Reflection;
using Omega.Tools.Experimental.Event;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Omega.Tools.Experimental.Events.Internals.EventManagers
{
    internal partial class UniversalEventManager<TEvent> : IEventManager<TEvent>
    {
        private Queue<IEvent> _queueEvents;
        private List<IEventHandler<TEvent>> _eventHandlers;

        public UniversalEventManager()
        {
            _queueEvents = new Queue<IEvent>();
            _eventHandlers = new List<IEventHandler<TEvent>>();
        }

        public void Event(TEvent arg)
        {
            var handlersOfEvent = _eventHandlers.ToArray();

            var @event = EventBuilder.CreateEvent(handlersOfEvent, arg);

            _queueEvents.Enqueue(@event);

            if (_queueEvents.Count == 1)
                EventMoveNext();
        }

        public void AddHandler(IEventHandler<TEvent> handler)
        {
            _eventHandlers.Add(handler);
        }

        public void RemoveHandler(IEventHandler<TEvent> handler)
        {
            _eventHandlers.Remove(handler);
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
    }
}