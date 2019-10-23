using System;
using Omega.Experimental.Event;

namespace Omega.Experimental.Event.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class EventCoverageAttribute : Attribute
    {
        public readonly EventCoverage Coverage;

        public EventCoverageAttribute(EventCoverage coverage)
        {
            Coverage = coverage;
        }
    }
}