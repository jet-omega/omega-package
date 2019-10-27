using System;

namespace Omega.Experimental.Event
{
    public sealed class ActionHandlerAdapter<TEvent> : IEventHandler<TEvent>
    {
        private readonly Action<TEvent> _action;

        public Action<TEvent> AdaptiveAction => _action;
        
        public ActionHandlerAdapter(Action<TEvent> action)
            => _action = action ?? throw new ArgumentNullException(nameof(action));

        public void OnEvent(TEvent arg)
            => _action(arg);

        public override bool Equals(object obj)
            => obj is ActionHandlerAdapter<TEvent> other && other._action == _action;

        public override int GetHashCode()
            => _action.GetHashCode();
    }
}