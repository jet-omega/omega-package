using System;
using System.Reflection;
using Omega.Tools.Experimental.Events;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Omega.Tools.Experimental.Event
{
    public sealed class UnityHandlerAdapter<TEvent> : IEventHandler<TEvent>
    {
        private readonly IEventHandler<TEvent> _handler;
        private readonly Object _targetObject;
        private readonly InvocationConvention _invocationConvention;
        public bool TargetObjectIsDestroyed => !_targetObject;

        public UnityHandlerAdapter(IEventHandler<TEvent> handler)
        {
            _handler = handler;
            _targetObject = handler as Object ?? throw new Exception();
            _invocationConvention =
                _targetObject.GetType().GetCustomAttribute<EventHandlerAttribute>()?.InvocationConvention ?? default;
        }

        public UnityHandlerAdapter(Object handler)
        {
            _handler = handler as IEventHandler<TEvent> ?? throw new Exception();
            _targetObject = handler;
            _invocationConvention =
                _targetObject.GetType().GetCustomAttribute<EventHandlerAttribute>()?.InvocationConvention ?? default;
        }
        
        internal UnityHandlerAdapter(IEventHandler<TEvent> handlerInterface, Object targetObject)
        {
            _handler = handlerInterface;
            _targetObject = targetObject;
            _invocationConvention =
                _targetObject.GetType().GetCustomAttribute<EventHandlerAttribute>()?.InvocationConvention ?? default;
        }
        
        public void Execute(TEvent arg)
        {
            if (TargetObjectIsDestroyed)
                if (_invocationConvention == InvocationConvention.PreventInvocationFromDestroyedObject)
                    throw new Exception(); //TODO
                else if (_invocationConvention == InvocationConvention.AllowInvocationFromDestroyedObjectButLogWarning)
                    Debug.LogWarning("TODO: write message"); //TODO

            _handler.Execute(arg);
        }

        public override bool Equals(object obj)
        {
            return obj is UnityHandlerAdapter<TEvent> other && other._targetObject == _targetObject;
        }

        public override int GetHashCode()
        {
            return _targetObject.GetHashCode() + _invocationConvention.GetHashCode();
        }
    }
}