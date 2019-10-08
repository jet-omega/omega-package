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
        private readonly LinkedList<IEventHandler<TEvent>> _handlers;

        public DefaultHandlersProvider()
        {
            _handlers = new LinkedList<IEventHandler<TEvent>>();
        }

        public void AddHandler(IEventHandler<TEvent> handler)
        {
            _handlers.AddLast(handler);
        }

        public void RemoveHandler(IEventHandler<TEvent> handler)
        {
            _handlers.Remove(handler);
        }

        public IEnumerable<IEventHandler<TEvent>> GetHandlers()
        {
            // TODO: Create revision collection?
            return _handlers.ToList();
        }
    }
}