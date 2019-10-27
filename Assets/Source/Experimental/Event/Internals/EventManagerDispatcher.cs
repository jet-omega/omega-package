namespace Omega.Experimental.Event.Internals
{
    internal static class EventManagerDispatcher<TEvent>
    {
        private static IEventManager<TEvent> _eventManager;

        public static IEventManager<TEvent> GetEventManager()
            => _eventManager ?? (_eventManager = EventManagerBuilder.Create<TEvent>());

        internal static IEventManager<TEvent> GetEventManagerHardInternal()
            => _eventManager;

        internal static void RemoveEventManagerInternal()
            => _eventManager = null;
    }
}