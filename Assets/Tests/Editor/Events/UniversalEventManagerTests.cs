using System;
using NUnit.Framework;

using Omega.Tools.Experimental.Events.Internals;
using Omega.Tools.Experimental.Events.Internals.EventManagers;

namespace Omega.Tools.Experimental.Events.Tests
{
    public sealed class UniversalEventManagerTests
    {
        [Test]
        public void AddHandlerShouldAddAndProcessHandlerTest()
        {
            bool handleFlag = false;
            var eventManager = new UniversalEventManager<TestEvent>();
            eventManager.AddHandler(_ => handleFlag = true);
            eventManager.Event(default);
            Assert.True(handleFlag);
        }
        
        [Test]
        public void RemoveHandlerShouldRemoveHandlerFromProcessPipelineTest()
        {
            var eventManager = new UniversalEventManager<TestEvent>();
            var handler = new Action<TestEvent>(_ => Assert.Fail());
         
            eventManager.AddHandler(handler);
            eventManager.RemoveHandler(handler);
            
            eventManager.Event(default);
        }

        private readonly struct TestEvent
        {
        }
    }
}