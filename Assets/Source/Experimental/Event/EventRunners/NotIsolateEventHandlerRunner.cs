using System.Collections.Generic;
using Omega.Tools.Experimental.Events;

namespace Omega.Tools.Experimental.Event
{
    internal class NotIsolateEventHandlerRunner<TEvent> : IEvent
    {
        private IEnumerable<IEventHandler<TEvent>> _handlers;
        private TEvent _eventArg;

        public NotIsolateEventHandlerRunner(IEnumerable<IEventHandler<TEvent>> handlers, TEvent eventArg)
        {
            _handlers = handlers;
            _eventArg = eventArg;
        }

        public void Release()
        {
            foreach (var handler in _handlers)
                handler.Execute(_eventArg);
        }
    }
}