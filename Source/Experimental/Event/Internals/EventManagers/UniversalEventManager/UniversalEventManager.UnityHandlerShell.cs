using System;
using Omega.Tools.Experimental.Event;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Omega.Tools.Experimental.Events.Internals.EventManagers
{
    internal partial class UniversalEventManager<TEvent>
    {
        private class UnityHandlerShell : IEventHandler<TEvent>
        {
            private Object _handlerObject;
            private IEventHandler<TEvent> _handler;
            private InvocationConvention _invocationConvention;

            public UnityHandlerShell(IEventHandler<TEvent> handler, InvocationConvention invocationConvention)
            {
                _handler = handler;
                _handlerObject = (Object) handler;
                _invocationConvention = invocationConvention;
            }
            
            public void Execute(TEvent arg)
            {
                if (!_handlerObject)
                    if (_invocationConvention == InvocationConvention.PreventInvocationFromDestroyedObject)
                        throw new Exception();
                    else if (_invocationConvention ==
                             InvocationConvention.AllowInvocationFromDestroyedObjectButLogWarning)
                        Debug.LogWarning("");
                
                _handler.Execute(arg);
            }
        }
    }
}