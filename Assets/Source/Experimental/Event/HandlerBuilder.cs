using System;
using System.Reflection;
using UnityEngine;

namespace Omega.Tools.Experimental.Events
{
//    public static class HandlerBuilder
//    {
//        public static IEventHandler<T> ByAction<T>(Action action)
//            => new ActionHandler<T>(action);
//        public static IEventHandler<T> ByAction<T>(Action<T> action)
//            => new ActionHandlerWithParameter<T>(action);
//    }
//    
//    public sealed class ActionHandlerWithParameter<T> : IEventHandler<T>
//    {
//        private readonly Action<T> _action;
//
//        public ActionHandlerWithParameter(Action<T> action)
//            => _action = action ?? throw new ArgumentNullException(nameof(action));
//        
//        public void Execute(T arg) => _action(arg);
//
//        public override bool Equals(object obj)
//        {
//            return obj is ActionHandlerWithParameter<T> other && _action.Equals(other._action);
//        }
//
//        public override int GetHashCode()
//        {
//            return _action.GetHashCode();
//        }
//    }
//    
//    public sealed class ActionHandler<T> : IEventHandler<T>
//    {
//        private readonly Action _action;
//
//        public ActionHandler(Action action)
//            => _action = action ?? throw new ArgumentNullException(nameof(action));
//        
//        public void Execute(T arg) => _action();
//        
//        public override bool Equals(object obj)
//        {
//            return obj is ActionHandler<T> other && _action.Equals(other._action);
//        }
//
//        public override int GetHashCode()
//        {
//            return _action.GetHashCode();
//        }
//    }
}