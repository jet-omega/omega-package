using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Omega.Experimental.Event.Internals
{
    internal interface IEventManager<TEvent>
    {
        void AddHandler(IEventHandler<TEvent> handler);
        void RemoveHandler(IEventHandler<TEvent> handler);
        IEnumerable<IEventHandler<TEvent>> GetEventHandlers();
    }
}