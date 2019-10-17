namespace Omega.Tools.Experimental.Event
{
    public enum InvocationPolicy
    {
        PreventInvocationFromDestroyedObject = 0,
        AllowInvocationFromDestroyedObjectButLogError,
        AllowInvocationFromDestroyedObject
    }
}