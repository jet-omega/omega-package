using System;

namespace Omega.Tools.Experimental.Event
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Struct)]
    public sealed class EventHandlerAttribute : Attribute
    {
        public readonly InvocationPolicy InvocationPolicy;

        public EventHandlerAttribute(InvocationPolicy invocationPolicy)
        {
            InvocationPolicy = invocationPolicy;
        }
    }

    public enum InvocationPolicy
    {
        PreventInvocationFromDestroyedObject = 0,
        AllowInvocationFromDestroyedObjectButLogWarning,
        AllowInvocationFromDestroyedObject
    }
}