using System;

namespace Omega.Tools.Experimental.Events
{
    public interface IEventManagerActionHandlerProvider<TEvent>
    {
        void AddHandler(Action<TEvent> action);
        void RemoveHandler(Action<TEvent> action);
    }
}