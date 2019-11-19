using System.Collections;

namespace Omega.Routines
{
    internal sealed class EmptyRoutine : Routine
    {
        protected override IEnumerator RoutineUpdate() => null;
    }
}