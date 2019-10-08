using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

namespace Omega.Tools.Experimental.Events.Tests
{
    public class EventManagerTests
    {
        [Test]
        public void EventManagerShouldAddHandleTest()
        {
            var handler = HandlerBuilder.ByAction<EventManagerTestsEvent>(() => { });
            EventManager.AddHandler(handler);

            var handlersCollection =  HandlersDispatcher<EventManagerTestsEvent>.Provider.GetHandlers();
            Assert.True(handlersCollection.ToList().Contains(handler));
        }

        [Test]
        public void EventManagerShouldNotifyHandlersTest()
        {
            bool handlerFlag = false;
            EventManager.AddHandler<EventManagerTestsEvent>(() => handlerFlag = true);
            EventManager.Event(new EventManagerTestsEvent());

            Assert.True(handlerFlag);
        }

        [Test]
        public void EventManagerShouldNotNotifyHandlersUntilPastEventNotifyHasEndedTest()
        {
            var state = 0;
            EventManager.AddHandler<EventManagerTestsEvent>(() =>
            {
                state++;
                if (state == 1)
                {
                    EventManager.Event<EventManagerTestsEvent>(default);
                    Assert.AreEqual(state, 1);
                }
            });

            EventManager.Event<EventManagerTestsEvent>(default);

            Assert.AreEqual(state, 2);
        }

        [Test]
        public void EventManagerShouldNotNotifyRemovedHandlerTest()
        {
            var handler = EventManager.AddHandler<EventManagerTestsEvent>(Assert.Fail);
            EventManager.RemoveHandler(handler);
            EventManager.Event<EventManagerTestsEvent>(default);
        }

        [TearDown]
        [SetUp]
        public void ClearProvider()
        {
            HandlersDispatcher<EventManagerTestsEvent>.SetProvider(null);
        }
        
        private struct EventManagerTestsEvent
        {
        }
    }
}