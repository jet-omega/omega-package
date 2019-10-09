namespace Omega.Tools.Experimental.Events.Internals.EventManagers
{
    internal partial class UniversalEventManager<TEvent>
    {
        private interface IEvent
        {
            void Release();
        }
    }
}