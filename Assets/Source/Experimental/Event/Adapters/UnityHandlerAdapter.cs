using System;
using Omega.Package;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Omega.Experimental.Event
{
    public sealed class UnityHandlerAdapter<TEvent> : IEventHandler<TEvent>
    {
        private readonly IEventHandler<TEvent> _handler;
        private readonly Object _targetObject;
        private readonly InvocationPolicy _invocationPolicy;

        public Object TargetObject => _targetObject;
        public bool TargetObjectIsDestroyed => !_targetObject;
        public InvocationPolicy InvocationPolicy => _invocationPolicy;
        public IEventHandler<TEvent> AdaptiveHandler => _handler;

        public UnityHandlerAdapter(IEventHandler<TEvent> handler, InvocationPolicy invocationPolicy)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
            
            _targetObject = handler as Object ?? throw ExceptionHelper.HandlerIsNotInstanceOfUnityObject;

            _invocationPolicy = invocationPolicy;
        }

        public UnityHandlerAdapter(Object handler, InvocationPolicy invocationPolicy)
        {
            if (handler is null)
                throw new ArgumentNullException(nameof(handler));

            _handler = handler as IEventHandler<TEvent> ??
                       throw ExceptionHelper.ObjectIsNotInstanceOfIEventHandler(typeof(TEvent));

            _targetObject = handler;
            
            _invocationPolicy = invocationPolicy;
        }

        internal UnityHandlerAdapter(IEventHandler<TEvent> handlerInterface, InvocationPolicy invocationPolicy,
            Object targetObject)
        {
            _handler = handlerInterface;
            _targetObject = targetObject;
            _invocationPolicy = invocationPolicy;
        }

        public void OnEvent(TEvent arg)
        {
            if (TargetObjectIsDestroyed)
                if (_invocationPolicy == InvocationPolicy.PreventInvocationFromDestroyedObject)
                    throw ExceptionHelper.MethodCannotCalledWhenObjectIsDestroyed;
                else if (_invocationPolicy == InvocationPolicy.AllowInvocationFromDestroyedObjectButLogError)
                    Debug.LogWarning(ExceptionHelper.Messages.MethodWasCalledInTheDestroyedObject);

            _handler.OnEvent(arg);
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