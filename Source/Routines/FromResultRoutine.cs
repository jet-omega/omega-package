using System.Collections;

namespace Omega.Routines
{
    internal sealed class FromResultRoutine<T> : Routine<T>
    {
        public FromResultRoutine(T result) => SetResult(result);
        protected override IEnumerator RoutineUpdate() => null;
    }
}