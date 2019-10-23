using System;

namespace Omega.Experimental.Event.Attributes
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
}