using System;
using System.Collections.Generic;
using UnityEngine;

namespace Omega.Tools.Experimental.Event
{
    internal static class EventScheduler
    {
        private static Queue<IEvent> _queueEvents = new Queue<IEvent>();
        private static IEvent _current;

        public static void Schedule(IEvent @event)
        {
            _queueEvents.Enqueue(@event);

            if (_current == null)
                EventMoveNext();
        }

        private static void EventMoveNext()
        {
            while (_queueEvents.Count > 0)
            {
                _current = _queueEvents.Dequeue();
                try
                {
                    _current.Release();
                }
                catch (Exception e)
                {
                    _current = null;
                    _queueEvents.Clear();
                    throw;
                }
                
                _current = null;
            }
        }
        
        private enum EventState
        {
            
        }
    }

    internal interface IEvent
    {
        void Release();
    }
}