using System;

namespace Omega.Tools.Experimental.Events
{
    public static class HandlerBuilder
    {
        public static IEventHandler<T> ByAction<T>(Action action)
            => new ActionHandler<T>(action);
        public static IEventHandler<T> ByAction<T>(Action<T> action)
            => new ActionHandlerWithParameter<T>(action);
    }
    
    public class ActionHandlerWithParameter<T> : IEventHandler<T>
    {
        private readonly Action<T> _action;

        public ActionHandlerWithParameter(Action<T> action)
            => _action = action ?? throw new ArgumentNullException(nameof(action));
        
        public void Execute(T arg) => _action(arg);
    }
    
    public class ActionHandler<T> : IEventHandler<T>
    {
        private readonly Action _action;

        public ActionHandler(Action action)
            => _action = action ?? throw new ArgumentNullException(nameof(action));
        
        public void Execute(T arg) => _action();
    }
}