using System;
using System.Reflection;
using Omega.Tools.Experimental.Events;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Omega.Tools.Experimental.Event
{
    internal static class ActionHandlerAdapterBuilder
    {
        public static IEventHandler<TEvent> Build<TEvent>(Action<TEvent> action)
        {
            if(action.Target is Object)
                return new ActionHandlerUnityAdapter<TEvent>(action);
            
            return new ActionHandlerAdapter<TEvent>(action);
        }
    }
    
    internal sealed class ActionHandlerAdapter<TEvent> : IEventHandler<TEvent>
    {
        private readonly Action<TEvent> _action;

        public ActionHandlerAdapter(Action<TEvent> action)
            => _action = action ?? throw new ArgumentNullException(nameof(action));

        public void Execute(TEvent arg)
            => _action(arg);

        public override bool Equals(object obj)
            => obj is ActionHandlerAdapter<TEvent> other && other._action == _action;

        public override int GetHashCode()
            => _action.GetHashCode();
    }

    internal sealed class ActionHandlerUnityAdapter<TEvent> : IEventHandler<TEvent>
    {
        private readonly Action<TEvent> _action;
        private readonly Object _targetObject;
        private readonly InvocationConvention _invocationConvention;
        public bool TargetObjectIsDestroyed => !_targetObject;

        public ActionHandlerUnityAdapter(Action<TEvent> action)
        {
            if (!(action.Target is Object unityObject))
                throw new ArgumentException(nameof(action)); //TODO: add exception message

            _targetObject = unityObject;

            _action = action;
            _invocationConvention =
                action.Method.GetCustomAttribute<EventHandlerAttribute>()?.InvocationConvention ?? default;
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