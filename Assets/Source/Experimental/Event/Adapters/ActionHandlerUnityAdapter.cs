using System;
using Omega.Package;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Omega.Experimental.Event
{
    public sealed class ActionHandlerUnityAdapter<TEvent> : IEventHandler<TEvent>
    {
        private readonly Action<TEvent> _action;
        private readonly Object _targetObject;
        private readonly InvocationPolicy _invocationPolicy;

        public Object TargetObject => _targetObject;
        public bool TargetObjectIsDestroyed => !_targetObject;
        public InvocationPolicy InvocationPolicy => _invocationPolicy;
        public Action<TEvent> AdaptiveAction => _action;

        public ActionHandlerUnityAdapter(Action<TEvent> action, InvocationPolicy invocationPolicy)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            if (!(action.Target is Object unityObject))
                throw ExceptionHelper.ActionIsNotInstanceOfUnityObjectMethod;

            _targetObject = unityObject;

            _action = action;
            _invocationPolicy = invocationPolicy;
        }

        internal ActionHandlerUnityAdapter(Action<TEvent> action, Object target, InvocationPolicy invocationPolicy)
        {
            _targetObject = target;
            _action = action;
            _invocationPolicy = invocationPolicy;
        }

        public void OnEvent(TEvent arg)
        {
            if (TargetObjectIsDestroyed)
                if (_invocationPolicy == InvocationPolicy.PreventInvocationFromDestroyedObject)
                    throw ExceptionHelper.ActionCannotCalledWhenObjectIsDestroyed;
                else if (_invocationPolicy == InvocationPolicy.AllowInvocationFromDestroyedObjectButLogError)
                    Debug.LogError(ExceptionHelper.Messages.ActionWasCalledInTheDestroyedObject);

            _action(arg);
        }

        public override bool Equals(object obj)
            => obj is ActionHandlerUnityAdapter<TEvent> other
               && other._action == _action
               && other._targetObject == _targetObject; //TODO: Mb use ReferenceEquals?

        public override int GetHashCode()
            => _action.GetHashCode() + _targetObject.GetHashCode();
    }
}