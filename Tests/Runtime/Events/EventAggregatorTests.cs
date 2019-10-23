using System;
using NUnit.Framework;
using Omega.Experimental.Event.Internals;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Omega.Experimental.Event.Tests
{
    public class EventAggregatorTests
    {
        [Test]
        public void AddHandlerShouldAddUnityObjectHandlerTest()
        {
            bool flag = false;

            var gameObject = new GameObject(nameof(AddHandlerShouldAddUnityObjectHandlerTest));
            var handler = gameObject.AddComponent<TestMonoBehaviour>();
            handler.callback = () => flag = true;
            
            EventAggregator.AddHandler(handler);
            EventAggregator.Event(new TestEvent());

            Assert.True(flag);
            
            Object.DestroyImmediate(gameObject);
        }
        
        [Test]
        public void AddHandlerShouldRemoveUnityObjectHandlerTest()
        {
            var gameObject = new GameObject(nameof(AddHandlerShouldRemoveUnityObjectHandlerTest));
            var handler = gameObject.AddComponent<TestMonoBehaviour>();
            handler.callback = () => Assert.Fail();
            
            EventAggregator.AddHandler(handler);
            EventAggregator.RemoveHandler(handler);
            EventAggregator.Event(new TestEvent());

            Object.DestroyImmediate(gameObject);
        }

        [SetUp]
        [TearDown]
        public void ClearHandlers()
            => EventManagerDispatcher<TestEvent>.RemoveEventManagerInternal();
        
        private struct TestEvent
        {
        }

        private class TestMonoBehaviour : MonoBehaviour, IEventHandler<TestEvent>
        {
            public Action callback;

            public void OnEvent(TestEvent arg)
            {
                callback.Invoke();
            }
        }
    }
}