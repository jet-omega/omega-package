using System.Collections;

namespace Omega.Routines
{
    public sealed class CompletedRoutine : Routine
    {
        public CompletedRoutine()
        {
            ((IEnumerator) this).MoveNext();
        }

        protected override IEnumerator RoutineUpdate()
        {
            return null;
        }
    }
}