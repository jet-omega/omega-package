using System;
using System.Collections;
using System.Collections.Generic;
using Omega.Routines;
using UnityEngine;

namespace Omega.Experimental.Event.Internals
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

        public static Routine ExecuteAsync(IEvent @event)
        {
            return new EventExecuteRoutine(@event);
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
        
        private sealed class EventExecuteRoutine : Routine
        {
            private readonly IEvent _event;
            
            protected override IEnumerator RoutineUpdate()
            {
                Schedule(_event);

                while (_current == _event || QueueEvents.Contains(_event))
                    yield return null;
            }

            internal EventExecuteRoutine(IEvent @event)
            {
                _event = @event;
            }
        }
    }
}