namespace Omega.Experimental.Event
{
    public interface IEventHandler<TEvent>
    {
        void OnEvent(TEvent arg);
    }
}