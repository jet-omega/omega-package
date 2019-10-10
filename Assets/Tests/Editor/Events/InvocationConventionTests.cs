using System;
using System.Text.RegularExpressions;
using NUnit.Framework;
using Omega.Tools.Experimental.Event;
using Omega.Tools.Experimental.Events.Internals;
using UnityEngine;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace Omega.Tools.Experimental.Events.Tests
{
    public class InvocationConventionTests
    {
        [Test]
        public void EventManagerShouldNotifyActionWithDestroyedTargetTest()
        {
            var gameObject = new GameObject(nameof(EventManagerShouldNotifyActionWithDestroyedTargetTest));
            var target = gameObject.AddComponent<TestHelperMonoBehaviour>();

            EventAggregator.AddHandler<TestEvent>(target.ActionWithAllowInvocationFromDestroyedObject);

            Object.DestroyImmediate(gameObject);

            EventAggregator.Event(new TestEvent());

            Assert.True(target.invokedAllowInvocationFromDestroyedObject);
        }

        [Test]
        public void EventManagerShouldNotifyActionWithDestroyedTargetAndLogTest()
        {
            var gameObject = new GameObject(nameof(EventManagerShouldNotifyActionWithDestroyedTargetAndLogTest));
            var target = gameObject.AddComponent<TestHelperMonoBehaviour>();

            EventAggregator.AddHandler<TestEvent>(target.ActionWithAllowInvocationFromDestroyedObjectButLogWarning);

            Object.DestroyImmediate(gameObject);

            EventAggregator.Event(new TestEvent());

            LogAssert.Expect(LogType.Warning, new Regex("."));
            Assert.True(target.invokedAllowInvocationFromDestroyedObjectButLogWarning);
        }

        [Test]
        //TODO: "ThrowException", Exception?
        public void EventManagerShouldThrowExceptionWhenTryNotifyActionWithDestroyedTargetTest()
        {
            var gameObject =
                new GameObject(nameof(EventManagerShouldThrowExceptionWhenTryNotifyActionWithDestroyedTargetTest));
            var target = gameObject.AddComponent<TestHelperMonoBehaviour>();

            EventAggregator.AddHandler<TestEvent>(target.ActionWithPreventInvocationFromDestroyedObject);

            Object.DestroyImmediate(gameObject);

            Assert.Throws<Exception>(() => EventAggregator.Event(new TestEvent()));
            
            Assert.False(target.invokedPreventInvocationFromDestroyedObject);
        }
        
        [Test]
        //TODO: "ThrowException", Exception?
        public void EventManagerShouldThrowExceptionWhenTryNotifyActionWithDestroyedTargetWithoutAttributeTest()
        {
            var gameObject =
                new GameObject(nameof(EventManagerShouldThrowExceptionWhenTryNotifyActionWithDestroyedTargetWithoutAttributeTest));
            var target = gameObject.AddComponent<TestHelperMonoBehaviour>();

            EventAggregator.AddHandler<TestEvent>(target.ActionWithoutInvocationConvention);

            Object.DestroyImmediate(gameObject);

            Assert.Throws<Exception>(() => EventAggregator.Event(new TestEvent()));
            
            Assert.False(target.invokedWithoutInvocationConvention);
        }

        [SetUp]
        [TearDown]
        public void ClearHandlers()
            => EventManagerDispatcher<TestEvent>.RemoveEventManagerInternal();

        private struct TestEvent
        {
        }

        private class TestHelperMonoBehaviour : MonoBehaviour
        {
            public bool invokedAllowInvocationFromDestroyedObject;
            public bool invokedAllowInvocationFromDestroyedObjectButLogWarning;
            public bool invokedPreventInvocationFromDestroyedObject;
            public bool invokedWithoutInvocationConvention;

            [EventHandler(InvocationConvention.AllowInvocationFromDestroyedObject)]
            public void ActionWithAllowInvocationFromDestroyedObject(TestEvent e)
                => invokedAllowInvocationFromDestroyedObject = true;

            [EventHandler(InvocationConvention.AllowInvocationFromDestroyedObjectButLogWarning)]
            public void ActionWithAllowInvocationFromDestroyedObjectButLogWarning(TestEvent e)
                => invokedAllowInvocationFromDestroyedObjectButLogWarning = true;

            [EventHandler(InvocationConvention.PreventInvocationFromDestroyedObject)]
            public void ActionWithPreventInvocationFromDestroyedObject(TestEvent e)
                => invokedAllowInvocationFromDestroyedObjectButLogWarning = true;

            public void ActionWithoutInvocationConvention(TestEvent e)
                => invokedWithoutInvocationConvention = true;
        }
    }
}