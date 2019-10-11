using System.Collections.Generic;

namespace Omega.Tools.Experimental.Event
{
    internal static class EventScheduler
    { 
        private static Queue<IEvent> _queueEvents = new Queue<IEvent>();
        
        public static void Schedule(IEvent @event)
        {
            _queueEvents.Enqueue(@event);

            if (_queueEvents.Count == 1)
                EventMoveNext();
        }
        
        private static void EventMoveNext()
        {
            while (_queueEvents.Count != 0)
            {
                var @event = _queueEvents.Peek();
                @event.Release();
                _queueEvents.Dequeue();
            }
        }
    }
    
    internal interface IEvent
    {
        void Release();
    }
}