namespace Omega.Experimental.Event
{
    public enum InvocationPolicy
    {
        PreventInvocationFromDestroyedObject = 0,
        AllowInvocationFromDestroyedObjectButLogError,
        AllowInvocationFromDestroyedObject
    }
}