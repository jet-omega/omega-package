using System.Collections;

namespace Omega.Routines
{
    internal sealed class RoutineWithResult<T> : Routine<T>
    {
        public RoutineWithResult(T result) => SetResult(result);
        protected override IEnumerator RoutineUpdate() => null;
    }
}