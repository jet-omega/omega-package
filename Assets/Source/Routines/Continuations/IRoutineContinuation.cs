using System;

namespace Omega.Routines
{
    public interface IRoutineContinuation
    {
        bool TryContinue(out Exception continuationException);
    }
}