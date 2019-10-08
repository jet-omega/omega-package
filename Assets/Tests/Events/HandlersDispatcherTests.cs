using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using UnityEngine;

namespace Omega.Tools.Experimental.Events.Tests
{
    public class HandlersDispatcherTests
    {
        [Test]
        public void HandlersDispatcherProviderShouldReturnDefaultProvider()
        {
            var provider = HandlersDispatcher<HandlersDispatcherTestsEvent>.Provider;
            Assert.AreEqual(provider.GetType(), new DefaultHandlersProvider<HandlersDispatcherTestsEvent>().GetType());
            
            HandlersDispatcher<HandlersDispatcherTestsEvent>.SetProvider(null);
        }
        
        [Test]
        public void HandlersDispatcherSetProviderShouldSetupCustomProvider()
        {
            var provider = new DefaultHandlersProvider<HandlersDispatcherTestsEvent>();
            HandlersDispatcher<HandlersDispatcherTestsEvent>.SetProvider(provider);
            
            Assert.AreEqual(HandlersDispatcher<HandlersDispatcherTestsEvent>.Provider, provider);
            
            HandlersDispatcher<HandlersDispatcherTestsEvent>.SetProvider(null);
        }

        private struct HandlersDispatcherTestsEvent
        {
            public object someArg;
        }
    }
}