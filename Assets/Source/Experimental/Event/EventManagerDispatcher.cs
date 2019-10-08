using System;

namespace Omega.Tools.Experimental.Events
{
    public static class EventManagerDispatcher<TEvent>
    {
        private static bool _supportActionHandlers;
        private static IEventManager<TEvent> _eventManager;
        private static IActionHandlerInterface<TEvent> _eventManagerActionInterface;

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

        internal static IActionHandlerInterface<TEvent> GetEventManagerActionInterface()
        {
            EventManagerNullCheck();
            return _eventManagerActionInterface;
        }

        public static void SetEventManager(IEventManager<TEvent> eventManager)
        {
            _eventManager = eventManager ?? throw new ArgumentNullException(nameof(eventManager));

            _supportActionHandlers = _eventManager is IActionHandlerInterface<TEvent>;
            if (SupportActionHandlers)
                _eventManagerActionInterface = (IActionHandlerInterface<TEvent>) _eventManager;
        }

        private static void EventManagerNullCheck()
        {
            if (_eventManager == null)
                SetEventManager(Create());
        }

        private static IEventManager<TEvent> Create()
            => new DefaultEventManager<TEvent>();
        
        internal static void RemoveEventManagerInternal()
            => _eventManager = null;
    }
}