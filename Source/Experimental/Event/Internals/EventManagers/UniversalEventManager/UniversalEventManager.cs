using System.Collections.Generic;

namespace Omega.Experimental.Event.Internals.EventManagers
{
    internal class UniversalEventManager<TEvent> : IEventManager<TEvent>
    {
        private readonly List<IEventHandler<TEvent>> _eventHandlers;

        public UniversalEventManager()
        {
            _eventHandlers = new List<IEventHandler<TEvent>>();
        }

        public void AddHandler(IEventHandler<TEvent> handler)
        {
            _eventHandlers.Add(handler);
        }

        public void RemoveHandler(IEventHandler<TEvent> handler)
        {
            _eventHandlers.Remove(handler);
        }

        public IEnumerable<IEventHandler<TEvent>> GetEventHandlers()
        {
            return _eventHandlers.ToArray();
        }
    }
}