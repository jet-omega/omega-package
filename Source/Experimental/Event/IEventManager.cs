using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Omega.Tools.Experimental.Events
{
    public interface IEventManager<TEvent>
    {
        void Event(TEvent arg);
        void AddHandler(IEventHandler<TEvent> handler);
        void RemoveHandler(IEventHandler<TEvent> handler);
    }
}