using System;
using System.Collections;
using System.Linq;
using NUnit.Framework;
using Omega.Experimental.Event.Internals;
using UnityEngine.TestTools;

namespace Omega.Experimental.Event.Tests
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
            var eventManager = EventManagerDispatcher<EventManagerTestsEvent>.GetEventManager();
            var handler = new CustomActionHandler<EventManagerTestsEvent>();

            eventManager.AddHandler(handler);
            Assert.Contains(handler, eventManager.GetEventHandlers().ToArray());
        }

        [Test]
        public void EventManagerShouldNotNotifyHandlersUntilPastEventNotifyHasEndedTest()
        {
            var state = 0;
            EventAggregator.AddHandler<EventManagerTestsEvent>(_ =>
            {
                if (++state == 1)
                {
                    EventAggregator.Event<EventManagerTestsEvent>(default);
                    Assert.AreEqual(state, 1);
                }
            });

            EventAggregator.Event<EventManagerTestsEvent>(default);

            Assert.AreEqual(state, 2);
        }

        [Test]
        public void EventManagerShouldNotNotifyHandlersUntilPastEventNotifyHasEndedSecondTest()
        {
            var flag = false;
            EventAggregator.AddHandler<EventManagerTestsEvent>(_ =>
            {
                flag = true;
                EventAggregator.Event<EventManagerTestsSecondEvent>(default);
                flag = false;
            });

            EventAggregator.AddHandler<EventManagerTestsSecondEvent>(_ =>
            {
                if (flag)
                    Assert.Fail();
            });

            EventAggregator.Event<EventManagerTestsEvent>(default);

            EventManagerDispatcher<EventManagerTestsSecondEvent>.RemoveEventManagerInternal();
        }

        [UnityTest]
        public IEnumerator EventManagerShouldWaitCoroutineTest()
        {
            bool flag = false;

            EventAggregator.AddHandler((EventManagerTestsEvent e) =>
            {
                if (!flag)
                    EventAggregator.Event(new EventManagerTestsEvent());
                flag = true;
            });

            yield return EventAggregator.EventAsync<EventManagerTestsSecondEvent>(default);
        }

        [Test]
        public void EventManagerShouldNotNotifyRemovedHandlerTest()
        {
            var handler = new Action<EventManagerTestsEvent>(_ => Assert.Fail());

            EventAggregator.AddHandler(handler);
            EventAggregator.RemoveHandler(handler);

            EventAggregator.Event<EventManagerTestsEvent>(default);
        }

        [TearDown]
        [SetUp]
        public void ClearEventManager()
        {
            EventManagerDispatcher<EventManagerTestsEvent>.RemoveEventManagerInternal();
        }

        private sealed class CustomActionHandler<TEvent> : IEventHandler<TEvent>
        {
            public void OnEvent(TEvent arg)
            {
            }
        }

        private struct EventManagerTestsEvent
        {
        }

        private struct EventManagerTestsSecondEvent
        {
        }
    }
}