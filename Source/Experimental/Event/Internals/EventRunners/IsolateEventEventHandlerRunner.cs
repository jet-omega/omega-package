using System;
using System.Collections.Generic;
using UnityEngine;

namespace Omega.Experimental.Event.Internals
{
    internal class IsolateEventEventHandlerRunner<TEvent> : IEvent
    {
        private readonly IEnumerable<IEventHandler<TEvent>> _handlers;
        private readonly TEvent _eventArg;
        private readonly Action<Exception> _exceptionHandler;

        public IsolateEventEventHandlerRunner(IEnumerable<IEventHandler<TEvent>> handlers, TEvent arg,
            Action<Exception> exceptionHandler)
        {
            _handlers = handlers;
            _eventArg = arg;
            _exceptionHandler = exceptionHandler ?? Debug.LogException;
        }

        public void Release()
        {
            try
            {
                foreach (var handler in _handlers)
                    handler.OnEvent(_eventArg);
            }
            catch (Exception e)
            {
                _exceptionHandler(e);
            }
        }
    }
}