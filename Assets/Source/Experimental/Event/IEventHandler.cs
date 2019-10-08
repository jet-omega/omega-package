namespace Omega.Tools.Experimental.Events
{
    public interface IEventHandler<TEvent>
    {
        void Execute(TEvent arg);
    }
}