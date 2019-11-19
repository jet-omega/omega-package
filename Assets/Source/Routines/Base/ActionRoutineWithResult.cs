using System;
using System.Collections;

namespace Omega.Routines
{
    internal sealed class ActionRoutine<T> : Routine<T>
    {
        private Func<T> _action;

        public ActionRoutine(Func<T> action)
        {
            _action = action;
        }

        protected override IEnumerator RoutineUpdate()
        {
            var result = _action.Invoke();
            SetResult(result);
            yield break;
        }
    }
}