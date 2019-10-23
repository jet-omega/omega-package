using System.Reflection;
using Omega.Experimental.Event.Attributes;
using Omega.Experimental.Event.Internals.EventManagers;

namespace Omega.Experimental.Event.Internals
{
    /// <summary>
    /// Вспомогательный класс для создания EventManager`ов
    /// </summary>
    internal static class EventManagerBuilder
    {
        public static IEventManager<TEvent> Create<TEvent>()
        {
            var eventType = typeof(TEvent);

            var sceneEventAttribute = eventType.GetCustomAttribute<EventCoverageAttribute>();

            if (sceneEventAttribute == null || sceneEventAttribute.Coverage == EventCoverage.Global)
                return CreateGlobalEventManager<TEvent>();
            
            return CreateSceneEventManager<TEvent>();
        }

        private static IEventManager<TEvent> CreateGlobalEventManager<TEvent>()
        {
            var eventManager = new UniversalEventManager<TEvent>();
            return eventManager;
        }

        private static IEventManager<TEvent> CreateSceneEventManager<TEvent>()
        {
            var eventManager = new SceneEventManager<TEvent>();
            return eventManager;
        }
    }
}