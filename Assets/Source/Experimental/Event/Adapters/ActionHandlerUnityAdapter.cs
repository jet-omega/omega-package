using System;
using System.Reflection;
using Omega.Tools.Experimental.Events;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Omega.Tools.Experimental.Event
{
    public sealed class ActionHandlerUnityAdapter<TEvent> : IEventHandler<TEvent>
    {
        private readonly Action<TEvent> _action;
        private readonly Object _targetObject;
        private readonly InvocationConvention _invocationConvention;
        
        public Object TargetObject => _targetObject;
        public bool TargetObjectIsDestroyed => !_targetObject;
        public InvocationConvention InvocationPolicy => _invocationConvention;
        public Action<TEvent> AdaptiveAction => _action;

        public ActionHandlerUnityAdapter(Action<TEvent> action, InvocationConvention invocationConvention)
        {
            if(action == null)
                throw new ArgumentNullException(nameof(action)); 
            
            if (!(action.Target is Object unityObject))
                throw new ArgumentException(nameof(action)); //TODO: add exception message

            _targetObject = unityObject;

            _action = action;
            _invocationConvention = invocationConvention;
        }

        internal ActionHandlerUnityAdapter(Action<TEvent> action, Object target,InvocationConvention invocationConvention)
        {
            _targetObject = target;
            _action = action;
            _invocationConvention = invocationConvention;
        }
        
        public void Execute(TEvent arg)
        {
            if (TargetObjectIsDestroyed)
                if (_invocationConvention == InvocationConvention.PreventInvocationFromDestroyedObject)
                    throw new Exception(); //TODO
                else if (_invocationConvention == InvocationConvention.AllowInvocationFromDestroyedObjectButLogWarning)
                    Debug.LogWarning("TODO: write message"); //TODO

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