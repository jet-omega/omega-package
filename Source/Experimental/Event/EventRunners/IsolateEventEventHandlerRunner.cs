using System;
using System.Collections.Generic;
using Omega.Tools.Experimental.Events;
using UnityEngine;

namespace Omega.Tools.Experimental.Event
{
    internal class IsolateEventEventHandlerRunner<TEvent> : IEvent
    {
        private IEnumerable<IEventHandler<TEvent>> _handlers;
        private TEvent _eventArg;
        private Action<Exception> _exceptionHandler;

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
                    handler.Execute(_eventArg);
            }
            catch (Exception e)
            {
                _exceptionHandler(e);
            }
        }
    }
}