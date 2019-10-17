using System;
using Omega.Tools.Experimental.Event;

namespace Omega.Tools.Experimental.Event.Attributes
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