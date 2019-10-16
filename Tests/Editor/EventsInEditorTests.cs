﻿using System;
using NUnit.Framework;

namespace Omega.Tools.Experimental.Events.Tests
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