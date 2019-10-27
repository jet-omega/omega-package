using System.Collections.Generic;

namespace Omega.Experimental.Event.Internals
{
    internal class NotIsolateEventHandlerRunner<TEvent> : IEvent
    {
        private readonly IEnumerable<IEventHandler<TEvent>> _handlers;
        private readonly TEvent _eventArg;

        public NotIsolateEventHandlerRunner(IEnumerable<IEventHandler<TEvent>> handlers, TEvent eventArg)
        {
            _handlers = handlers;
            _eventArg = eventArg;
        }

        public void Release()
        {
            foreach (var handler in _handlers)
                handler.OnEvent(_eventArg);
        }
    }
}