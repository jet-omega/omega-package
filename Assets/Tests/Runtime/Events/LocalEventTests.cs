using NUnit.Framework;
using Omega.Tools.Experimental.Event;
using Omega.Tools.Experimental.Events.Attributes;
using UnityEngine.SceneManagement;

namespace Omega.Tools.Experimental.Events.Tests
{
    public class SceneEventTests
    {
        /*
         * По хорошему нужно использовать UnityTest, но по каким то причинам, при переключении сцен UnityTest перестают
         * корректно работать
         */
        [Test]
        [MaxTime(2500)]
        public void EventManagerDispatcherShouldCreateNewEventManagerWhenSceneWereSwitchedTest()
        {
            int handlerInvocationCount = 0;

            var initialScene = SceneManager.CreateScene("initial",
                new CreateSceneParameters {localPhysicsMode = LocalPhysicsMode.None});
            initialScene.name = $"Initial scene for {nameof(EventManagerDispatcherShouldCreateNewEventManagerWhenSceneWereSwitchedTest)}";
            SceneManager.SetActiveScene(initialScene);
            
            EventAggregator.AddHandler<TestSceneWithAttributeEvent>(_ => handlerInvocationCount++);
            EventAggregator.Event<TestSceneWithAttributeEvent>(default);

            Assert.AreEqual(1, handlerInvocationCount);

            var secondScene = SceneManager.CreateScene("second",
                new CreateSceneParameters {localPhysicsMode = LocalPhysicsMode.None});
            secondScene.name = $"Second scene for {nameof(EventManagerDispatcherShouldCreateNewEventManagerWhenSceneWereSwitchedTest)}";
            SceneManager.SetActiveScene(secondScene);

            EventAggregator.Event<TestSceneWithAttributeEvent>(default);

            Assert.AreEqual(1, handlerInvocationCount);
        }


        [EventCoverage(EventCoverage.Scene)]
        private struct TestSceneWithAttributeEvent
        {
        }
    }
}