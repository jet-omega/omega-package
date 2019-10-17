namespace Omega.Tools.Experimental.Event
{
    public interface IEventHandler<TEvent>
    {
        void OnEvent(TEvent arg);
    }
}