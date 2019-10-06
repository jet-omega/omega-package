namespace Omega.Tools.Experimental.Events
{
    public interface IEventHandler<T>
    {
        void Execute(T arg);
    }
}