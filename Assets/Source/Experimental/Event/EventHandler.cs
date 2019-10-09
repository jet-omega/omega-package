using System;

namespace Omega.Tools.Experimental.Event
{
    public sealed class EventHandlerAttribute : Attribute
    {
        public readonly InvocationConvention InvocationConvention;

        public EventHandlerAttribute(InvocationConvention invocationConvention)
        {
            InvocationConvention = invocationConvention;
        }

        public EventHandlerAttribute()
        {
        }
    }

    public enum InvocationConvention
    {
        PreventInvocationFromDestroyedObject = 0,
        AllowInvocationFromDestroyedObjectButLogWarning,
        AllowInvocationFromDestroyedObject
    }
}