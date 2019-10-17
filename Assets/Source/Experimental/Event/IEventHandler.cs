namespace Omega.Tools.Experimental.Event
{
    public interface IEventHandler<TEvent>
    {
        void Execute(TEvent arg);
    }
}