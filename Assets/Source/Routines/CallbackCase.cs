using System;

namespace Omega.Routines
{
    [Flags]
    public enum CallbackCase
    {
        Complete = 1,
        Cancel = 1 << 1,
        Error = 1 << 2,
        NotComplete =  Cancel | Error,
        Any = Complete | Cancel | Error
    }
}