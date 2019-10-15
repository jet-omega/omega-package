using System;
using System.Reflection;
using Omega.Tools.Experimental.Event;
using Omega.Tools.Experimental.Events.Attributes;
using Omega.Tools.Experimental.Events.Internals.EventManagers;

namespace Omega.Tools.Experimental.Events.Internals
{
    /// <summary>
    /// Вспомогательный класс для создания EventManager`ов
    /// </summary>
    internal static class EventManagerBuilder
    {
        public static IEventManager<TEvent> Build<TEvent>()
        {
            var eventType = typeof(TEvent);

            var sceneEventAttribute = eventType.GetCustomAttribute<EventCoverageAttribute>();

            if (sceneEventAttribute == null || sceneEventAttribute.Coverage == EventCoverage.Global)
                return BuildGlobalEventManager<TEvent>();
            
            return BuildSceneEventManager<TEvent>();
        }

        private static IEventManager<TEvent> BuildGlobalEventManager<TEvent>()
        {
            var eventManager = new UniversalEventManager<TEvent>();
            return eventManager;
        }

        private static IEventManager<TEvent> BuildSceneEventManager<TEvent>()
        {
            var eventManager = new SceneEventManager<TEvent>();
            return eventManager;
        }
    }
}