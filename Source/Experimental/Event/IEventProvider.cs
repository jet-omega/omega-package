using System.Collections.Generic;

namespace Omega.Tools.Experimental.Events
{
    internal static class EventBuilder
    {
        public static IEvent CreateEvent<T>(IEnumerable<IEventHandler<T>> handlers, T arg)
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

    public interface IEvent
    {
        void Release();
    }
}