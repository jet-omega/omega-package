using System;
using NUnit.Framework;
using Omega.Experimental.Event;

namespace Omega.Experimental.Event.Tests
{
    public class EventsInEditorTests
    {
        [Test]
        public void EventAggregatorShouldThrowPlatformExceptionWhenHisApiUsingInEditModeTest()
        {
            Assert.Throws<PlatformNotSupportedException>(() => EventAggregator.AddHandler<TestEvent>(_ => Assert.Fail()));
            Assert.Throws<PlatformNotSupportedException>(() => EventAggregator.Event<TestEvent>(default));
            Assert.Throws<PlatformNotSupportedException>(() => EventAggregator.RemoveHandler<TestEvent>(_ => Assert.Fail()));
            Assert.Throws<PlatformNotSupportedException>(() => EventAggregator.EventAsync<TestEvent>(default));
        }

        private struct TestEvent
        {
        }
    }
}