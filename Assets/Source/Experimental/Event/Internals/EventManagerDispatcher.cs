using System;
using Omega.Tools.Experimental.Events;
using Omega.Tools.Experimental.Events.Internals;

namespace Omega.Tools.Experimental.Event.Internals
{
    internal static class EventManagerDispatcher<TEvent>
    {
        private static bool _supportActionHandlers;
        private static IEventManager<TEvent> _eventManager;
        
        public static bool SupportActionHandlers
        {
            get
            {
                EventManagerNullCheck();
                return _supportActionHandlers;
            }
        }

        public static IEventManager<TEvent> GetEventManager()
        {
            EventManagerNullCheck();
            return _eventManager;
        }

        internal static void SetEventManagerInternal(IEventManager<TEvent> eventManager)
        {
            _eventManager = eventManager ?? throw new ArgumentNullException(nameof(eventManager));
        }

        private static void EventManagerNullCheck()
        {
            if (_eventManager == null)
                SetEventManagerInternal(Create());
        }

        private static IEventManager<TEvent> Create()
            => EventManagerBuilder.Build<TEvent>();
        
        internal static void RemoveEventManagerInternal()
            => _eventManager = null;
    }
}