using System;
using JetBrains.Annotations;
using Omega.Tools.Experimental.Events.Internals;

namespace Omega.Tools.Experimental.Events.Internals
{
    internal static class EventManagerDispatcher<TEvent>
    {
        private static bool _supportActionHandlers;
        private static IEventManager<TEvent> _eventManager;
        private static IEventManagerActionHandlerProvider<TEvent> _eventManagerActionInterface;

        internal static bool InUndefinedEventManager => _eventManager == null;
        
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

        internal static IEventManagerActionHandlerProvider<TEvent> GetEventManagerActionInterface()
        {
            EventManagerNullCheck();
            return _eventManagerActionInterface;
        }

        internal static void SetEventManagerInternal(IEventManager<TEvent> eventManager)
        {
            _eventManager = eventManager ?? throw new ArgumentNullException(nameof(eventManager));

            _supportActionHandlers = _eventManager is IEventManagerActionHandlerProvider<TEvent>;
            if (SupportActionHandlers)
                _eventManagerActionInterface = (IEventManagerActionHandlerProvider<TEvent>) _eventManager;
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