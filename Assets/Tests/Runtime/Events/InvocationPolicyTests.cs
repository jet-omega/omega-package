using System;
using System.Text.RegularExpressions;
using NUnit.Framework;
using Omega.Tools.Experimental.Event;
using Omega.Tools.Experimental.Event.Internals;
using UnityEngine;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace Omega.Tools.Experimental.Events.Tests
{
    public class InvocationPolicyTests
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
        public void EventManagerShouldLogExceptionWhenTryNotifyActionWithDestroyedTargetTest()
        {
            var gameObject =
                new GameObject(nameof(EventManagerShouldLogExceptionWhenTryNotifyActionWithDestroyedTargetTest));
            var target = gameObject.AddComponent<TestHelperMonoBehaviour>();

            EventAggregator.AddHandler<TestEvent>(target.ActionWithPreventInvocationFromDestroyedObject);

            Object.DestroyImmediate(gameObject);

            LogAssert.Expect(LogType.Exception, new Regex("."));

            EventAggregator.Event(new TestEvent());

            Assert.False(target.invokedPreventInvocationFromDestroyedObject);
        }

        [Test]
        //TODO: "ThrowException", Exception?
        public void EventManagerShouldThrowExceptionWhenTryNotifyActionWithDestroyedTargetWithoutAttributeTest()
        {
            var gameObject =
                new GameObject(
                    nameof(EventManagerShouldThrowExceptionWhenTryNotifyActionWithDestroyedTargetWithoutAttributeTest));
            var target = gameObject.AddComponent<TestHelperMonoBehaviour>();

            EventAggregator.AddHandler<TestEvent>(target.ActionWithoutInvocationPolicy);

            Object.DestroyImmediate(gameObject);

            LogAssert.Expect(LogType.Exception, new Regex("."));

            EventAggregator.Event(new TestEvent());

            Assert.False(target.invokedWithoutInvocationPolicy);
        }

        [Test]
        public void ActionHandlerUnityAdapterShouldThrowInvalidCastException()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<InvalidCastException>(() => new ActionHandlerUnityAdapter<TestEvent>(e => Assert.Fail(), default));
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
            public bool invokedWithoutInvocationPolicy;

            [EventHandler(InvocationPolicy.AllowInvocationFromDestroyedObject)]
            public void ActionWithAllowInvocationFromDestroyedObject(TestEvent e)
                => invokedAllowInvocationFromDestroyedObject = true;

            [EventHandler(InvocationPolicy.AllowInvocationFromDestroyedObjectButLogWarning)]
            public void ActionWithAllowInvocationFromDestroyedObjectButLogWarning(TestEvent e)
                => invokedAllowInvocationFromDestroyedObjectButLogWarning = true;

            [EventHandler(InvocationPolicy.PreventInvocationFromDestroyedObject)]
            public void ActionWithPreventInvocationFromDestroyedObject(TestEvent e)
                => invokedAllowInvocationFromDestroyedObjectButLogWarning = true;

            public void ActionWithoutInvocationPolicy(TestEvent e)
                => invokedWithoutInvocationPolicy = true;
        }
    }
}