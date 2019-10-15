using System;

namespace Omega.Tools.Experimental.Event
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

    public enum EventHandling
    {
        NotIsolate,
        IsolateEvent,
        IsolateHandlers,
    }
}