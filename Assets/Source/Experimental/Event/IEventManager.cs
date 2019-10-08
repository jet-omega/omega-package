using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Omega.Tools.Runtime.Tests")]

namespace Omega.Tools.Experimental.Events
{
    public interface IEventManager<TEvent>
    {
        void Event(TEvent arg);
        void AddHandler(IEventHandler<TEvent> handler);
        void RemoveHandler(IEventHandler<TEvent> handler);
    }

    public interface IActionHandlerInterface<TEvent>
    {
        void AddHandler(Action<TEvent> action);
        void RemoveHandler(Action<TEvent> action);
    }

    public class DefaultEventManager<TEvent> : IEventManager<TEvent>, IActionHandlerInterface<TEvent>
    {
        private Queue<IEvent> _queueProviders = new Queue<IEvent>();
        private IHandlersProvider<TEvent> _handlerProvider = new DefaultHandlersProvider<TEvent>();
        private Dictionary<Action<TEvent>, IEventHandler<TEvent>> _actionHandlers = new Dictionary<Action<TEvent>, IEventHandler<TEvent>>();
        
        public void Event(TEvent arg)
        {
            var handlersOfEvent = _handlerProvider.GetHandlers();

            var @event = EventBuilder.CreateEvent(handlersOfEvent, arg);

            _queueProviders.Enqueue(@event);

            if (_queueProviders.Count == 1)
                EventMoveNext();
        }

        public void AddHandler(IEventHandler<TEvent> handler)
        {
            _handlerProvider.AddHandler(handler);
        }

        public void RemoveHandler(IEventHandler<TEvent> handler)
        {
            _handlerProvider.RemoveHandler(handler);
        }

        private void EventMoveNext()
        {
            while (TryPeek(_queueProviders, out var provider))
            {
                provider.Release();
                _queueProviders.Dequeue();
            }
        }

        //TODO: Remove or create queue extensions
        private static bool TryPeek<T>(Queue<T> queue, out T value)
        {
            if (ReferenceEquals(queue, null))
                throw new NullReferenceException(nameof(queue));

            if (queue.Count == 0)
            {
                value = default;
                return false;
            }

            value = queue.Peek();
            return true;
        }

        public void AddHandler(Action<TEvent> action)
        {
            if(_actionHandlers.ContainsKey(action))
                throw new OverflowException(nameof(action));
            
            var handler = new ActionHandler<TEvent>(action);
            _actionHandlers.Add(action, handler);
            
            AddHandler(handler);
        }

        public void RemoveHandler(Action<TEvent> action)
        {
            if(!_actionHandlers.TryGetValue(action, out var handler))
                return;
            
            RemoveHandler(handler);
            _actionHandlers.Remove(action);
        }

        public sealed class ActionHandler<T> : IEventHandler<T>
        {
            private readonly Action<T> _action;

            public ActionHandler(Action<T> action)
                => _action = action ?? throw new ArgumentNullException(nameof(action));

            public void Execute(T arg) => _action(arg);

            public override bool Equals(object obj)
            {
                return obj is ActionHandler<T> other && _action.Equals(other._action);
            }

            public override int GetHashCode()
            {
                return _action.GetHashCode();
            }
        }
    }
}