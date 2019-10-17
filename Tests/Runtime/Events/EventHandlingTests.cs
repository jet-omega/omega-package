using System;
using System.Text.RegularExpressions;
using NUnit.Framework;
using Omega.Tools.Experimental.Event.Attributes;
using Omega.Tools.Experimental.Event.Internals;
using UnityEngine;
using UnityEngine.TestTools;

namespace Omega.Tools.Experimental.Event.Tests
{
    public class EventHandlingTests
    {
        [Test]
        public void ShouldLogExceptionAndResetQueueEventsWhenHandlerThrowExceptionTest()
        {
            EventAggregator.AddHandler<NotIsolateEvent>(_ => EventAggregator.Event<IsolateEvent>(new IsolateEvent()));
            EventAggregator.AddHandler<IsolateEvent>(_ => Assert.Fail());
            
            EventAggregator.AddHandler<NotIsolateEvent>(_ => throw new Exception());
            EventAggregator.AddHandler<NotIsolateEvent>(_ => Assert.Fail());

            LogAssert.Expect(LogType.Exception, new Regex("."));
            EventAggregator.Event(new NotIsolateEvent());

            EventManagerDispatcher<NotIsolateEvent>.RemoveEventManagerInternal();
            EventManagerDispatcher<IsolateEvent>.RemoveEventManagerInternal();
        }
        
        [Test]
        public void ShouldLogExceptionAndShouldNotResetQueueEventsWhenHandlerThrowExceptionTest()
        {
            bool flag = false;
            
            EventAggregator.AddHandler<NotIsolateEvent>(_ => flag = true);
            
            EventAggregator.AddHandler<IsolateEvent>(_ => EventAggregator.Event(new NotIsolateEvent()));
            EventAggregator.AddHandler<IsolateEvent>(_ => throw new Exception());
            EventAggregator.AddHandler<IsolateEvent>(_ => Assert.Fail());

            LogAssert.Expect(LogType.Exception, new Regex("."));
            EventAggregator.Event(new IsolateEvent());
            Assert.True(flag);

            EventManagerDispatcher<NotIsolateEvent>.RemoveEventManagerInternal();
            EventManagerDispatcher<IsolateEvent>.RemoveEventManagerInternal();
        }

        [Test]
        public void ShouldLogExceptionAndShouldNotResetQueueEventsAndNotStopCurrentEventWhenHandlerThrowExceptionTest()
        {
            bool flagFromIsolateHandlers = false;
            bool flagFromIsolateEvent = false;
            
            EventAggregator.AddHandler<IsolateHandlersEvent>(_ => throw new Exception());
            EventAggregator.AddHandler<IsolateHandlersEvent>(_ => flagFromIsolateHandlers = true);
            EventAggregator.AddHandler<IsolateHandlersEvent>(_ => EventAggregator.Event(new IsolateEvent()));
            EventAggregator.AddHandler<IsolateEvent>(_ => flagFromIsolateEvent = true);

            LogAssert.Expect(LogType.Exception, new Regex("."));
            EventAggregator.Event(new IsolateHandlersEvent());
            
            Assert.True(flagFromIsolateHandlers);
            Assert.True(flagFromIsolateEvent);

            EventManagerDispatcher<IsolateHandlersEvent>.RemoveEventManagerInternal();
            EventManagerDispatcher<IsolateEvent>.RemoveEventManagerInternal();
        }

        [EventHandling(EventHandling.NotIsolate)]
        private struct NotIsolateEvent
        {
        }

        [EventHandling(EventHandling.IsolateEvent)]
        private struct IsolateEvent
        {
        }

        [EventHandling(EventHandling.IsolateHandlers)]
        private struct IsolateHandlersEvent
        {
        }
    }
}