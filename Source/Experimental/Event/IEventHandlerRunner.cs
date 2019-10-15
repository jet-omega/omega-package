using System;
using System.Collections.Generic;
using Omega.Tools.Experimental.Events;
using UnityEngine;

namespace Omega.Tools.Experimental.Event
{
    internal class SafeEventHandlerRunner<TEvent> : IEvent
    {
        private IEnumerable<IEventHandler<TEvent>> _handlers;
        private TEvent _eventArg;

        public SafeEventHandlerRunner(IEnumerable<IEventHandler<TEvent>> handlers, TEvent eventArg)
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

    internal class HierarchyEventHandlerRunner<TEvent> : IEvent
    {
        private IEnumerable<IEventHandler<TEvent>> _handlers;
        private TEvent _eventArg;
        private Action<Exception> _exceptionHandler;

        public HierarchyEventHandlerRunner(IEnumerable<IEventHandler<TEvent>> handlers, TEvent arg,
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

    internal class AggressiveEventHandlerRunner<TEvent> : IEvent
    {
        private IEnumerable<IEventHandler<TEvent>> _handlers;
        private TEvent _eventArg;
        private Action<IEventHandler<TEvent>, Exception> _exceptionHandler;

        public AggressiveEventHandlerRunner(IEnumerable<IEventHandler<TEvent>> handlers, TEvent arg,
            Action<IEventHandler<TEvent>, Exception> exceptionHandler)
        {
            _handlers = handlers;
            _eventArg = arg;
            _exceptionHandler = exceptionHandler ?? ((h, e) => Debug.LogException(e));
        }

        public void Release()
        {
            foreach (var handler in _handlers)
                try
                {
                    handler.Execute(_eventArg);
                }
                catch (Exception e)
                {
                    _exceptionHandler(handler, e);
                }
        }
    }
}