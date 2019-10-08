using System;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

namespace Omega.Tools.Experimental.Events.Tests
{
    public class EventManagerTests
    {
        [Test]
        public void EventManagerShouldCreateDefaultInstanceForEventTypeTest()
        {
            ClearEventManager();
            var eventManager = EventManagerDispatcher<EventManagerTestsEvent>.GetEventManager();
            Assert.NotNull(eventManager);
        }

        [Test]
        public void EventManagerShouldAddHandleTest()
        {
            bool handlerFlag = false;

            var eventManager = EventManagerDispatcher<EventManagerTestsEvent>.GetEventManager();
            var handler = new ActionHandler<EventManagerTestsEvent>
            {
                Action = _ => handlerFlag = true
            };

            eventManager.AddHandler(handler);
            eventManager.Event(default);

            Assert.True(handlerFlag);
        }

        [Test]
        public void EventManagerShouldNotNotifyHandlersUntilPastEventNotifyHasEndedTest()
        {
            var state = 0;
            EventManager.AddHandler<EventManagerTestsEvent>(_ =>
            {
                if (++state == 1)
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
            var handler = new Action<EventManagerTestsEvent>(_ => Assert.Fail());
            
            EventManager.AddHandler<EventManagerTestsEvent>(handler);
            EventManager.RemoveHandler(handler);
            
            EventManager.Event<EventManagerTestsEvent>(default);
        }

        [TearDown]
        [SetUp]
        public void ClearEventManager()
            => EventManagerDispatcher<EventManagerTestsEvent>.RemoveEventManagerInternal();

        private sealed class ActionHandler<TEvent> : IEventHandler<TEvent>
        {
            public Action<TEvent> Action;

            public void Execute(TEvent arg)
                => Action?.Invoke(arg);
        }

        private struct EventManagerTestsEvent
        {
        }
    }
}