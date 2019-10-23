using System;

namespace Omega.Experimental.Event.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public sealed class EventHandlingAttribute : Attribute
    {
        public readonly EventHandling Handling;

        public EventHandlingAttribute(EventHandling handling)
        {
            Handling = handling;
        }
    }
}