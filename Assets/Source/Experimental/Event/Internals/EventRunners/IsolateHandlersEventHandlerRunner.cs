using System.Collections.Generic;
using UnityEngine;

namespace Omega.Experimental.Event.Internals
{
    internal class IsolateHandlersEventHandlerRunner<TEvent> : IEvent
    {
        private readonly IEnumerable<IEventHandler<TEvent>> _handlers;
        private readonly TEvent _eventArg;

        public IsolateHandlersEventHandlerRunner(IEnumerable<IEventHandler<TEvent>> handlers, TEvent arg)
        {
            _handlers = handlers;
            _eventArg = arg;
        }

        public void Release()
        {
            foreach (var handler in _handlers)
                handler.OnEvent(_eventArg);
        }
    }
}