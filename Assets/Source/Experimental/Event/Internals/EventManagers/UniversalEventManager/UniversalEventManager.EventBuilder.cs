using System;
using System.Collections.Generic;
using Omega.Tools.Experimental.Event;

namespace Omega.Tools.Experimental.Events.Internals.EventManagers
{
    internal partial class UniversalEventManager<TEvent>
    {
        private static class EventBuilder
        {
            internal static IEvent CreateEventInternal<T>(IEnumerable<IEventHandler<T>> handlers, T arg)
            {
                var @event = new Event<T>(handlers, arg);
                return @event;
            }

            private sealed class Event<T> : IEvent
            {
                private IEnumerable<IEventHandler<T>> _handlers;
                private T _arg;

                public Event(IEnumerable<IEventHandler<T>> handlers, T arg)
                {
                    _handlers = handlers;
                    _arg = arg;
                }

                public void Release()
                {
                    foreach (var handler in _handlers)
                        handler.Execute(_arg);
                }
            }
        }
    }
}