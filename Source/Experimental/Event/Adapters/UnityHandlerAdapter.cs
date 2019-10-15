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

        public Object TargetObject => _targetObject;
        public bool TargetObjectIsDestroyed => !_targetObject;
        public InvocationConvention InvocationPolicy => _invocationConvention;
        public IEventHandler<TEvent> AdaptiveHandler => _handler;

        public UnityHandlerAdapter(IEventHandler<TEvent> handler, InvocationConvention invocationPolicy)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
            _targetObject = handler as Object ?? throw new InvalidCastException();
            _invocationConvention = invocationPolicy;
        }

        public UnityHandlerAdapter(Object handler, InvocationConvention invocationPolicy)
        {
            if(ReferenceEquals(handler,null))
                throw new ArgumentNullException(nameof(handler));
                
            _handler = handler as IEventHandler<TEvent> ?? throw new InvalidCastException();
            _targetObject = handler;
            _invocationConvention = invocationPolicy;
        }

        internal UnityHandlerAdapter(IEventHandler<TEvent> handlerInterface, InvocationConvention invocationPolicy,
            Object targetObject)
        {
            _handler = handlerInterface;
            _targetObject = targetObject;
            _invocationConvention = invocationPolicy;
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
            return _targetObject.GetHashCode();
        }
    }
}