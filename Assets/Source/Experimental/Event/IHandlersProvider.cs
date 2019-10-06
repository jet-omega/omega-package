using System.Collections.Generic;
using System.Linq;

namespace Omega.Tools.Experimental.Events
{
    public interface IHandlersProvider<TEvent>
    {
        void AddHandler(IEventHandler<TEvent> handler);
        void RemoveHandler(IEventHandler<TEvent> handler);
        
        IEnumerable<IEventHandler<TEvent>> GetHandlers();
    }

    public sealed class DefaultHandlersProvider<TEvent> : IHandlersProvider<TEvent>
    {
        private LinkedList<IEventHandler<TEvent>> _mainHandlers;

        public DefaultHandlersProvider()
        {
            _mainHandlers = new LinkedList<IEventHandler<TEvent>>();
        }

        public void AddHandler(IEventHandler<TEvent> handler)
        {
            _mainHandlers.AddLast(handler);
        }

        public void RemoveHandler(IEventHandler<TEvent> handler)
        {
            _mainHandlers.Remove(handler);
        }

        public IEnumerable<IEventHandler<TEvent>> GetHandlers()
        {
            // TODO: [CRITICAL] Create revision collection 
            return _mainHandlers.ToList();
        }
    }
}