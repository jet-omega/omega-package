using System;
using System.Text.RegularExpressions;
using NUnit.Framework;
using Omega.Experimental.Event.Attributes;
using Omega.Experimental.Event.Internals;
using UnityEngine;
using UnityEngine.TestTools;

namespace Omega.Experimental.Event.Tests
{
    public class EventHandlingTests
    {
        [Test]
        public void ShouldLogExceptionAndResetQueueEventsWhenHandlerThrowExceptionTest()
        {
            string keyMessage = nameof(keyMessage); 
            
            EventAggregator.AddHandler<NotIsolateEvent>(_ => EventAggregator.Event<IsolateEvent>(new IsolateEvent()));
            EventAggregator.AddHandler<IsolateEvent>(_ => Assert.Fail());
            
            EventAggregator.AddHandler<NotIsolateEvent>(_ => throw new Exception(keyMessage));
            EventAggregator.AddHandler<NotIsolateEvent>(_ => Assert.Fail());

            LogAssert.Expect(LogType.Exception, new Regex(".keyMessage"));
            EventAggregator.Event(new NotIsolateEvent());

            EventManagerDispatcher<NotIsolateEvent>.RemoveEventManagerInternal();
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