using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Omega.Tools.Experimental.Event.Internals
{
    internal static class EventScheduler
    {
        private static readonly Queue<IEvent> QueueEvents = new Queue<IEvent>();
        private static IEvent _current;

        public static void Schedule(IEvent @event)
        {
            QueueEvents.Enqueue(@event);

            if (_current == null)
                EventMoveNext();
        }

        public static IEnumerator ExecuteAsync(IEvent @event)
        {
            Schedule(@event);

            while (_current == @event || QueueEvents.Contains(@event))
                yield return null;
        }
        
        private static void EventMoveNext()
        {
            while (QueueEvents.Count > 0)
            {
                _current = QueueEvents.Dequeue();
                try
                {
                    _current.Release();
                }
                catch (Exception e)
                {
                    _current = null;
                    QueueEvents.Clear();
                    Debug.LogException(e);
                    return;
                }
                
                _current = null;
            }
        }
        
        private enum EventState
        {
            
        }
    }
}