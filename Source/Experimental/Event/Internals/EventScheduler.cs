using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Omega.Tools.Experimental.Event.Internals
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

        public static IEnumerator ExecuteAsync(IEvent @event)
        {
            Schedule(@event);

            while (_current == @event || _queueEvents.Contains(@event))
                yield return null;
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
                    Debug.LogException(e);
//                    var schedulerException = new Exception("Some handler throw exception" , e);
//                    Debug.LogException(schedulerException);
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