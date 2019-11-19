using System;
using System.Collections;

namespace Omega.Routines
{
    public sealed class ActionRoutine : Routine
    {
        private Action _action;

        public ActionRoutine(Action action)
        {
            _action = action;
        }

        protected override IEnumerator RoutineUpdate()
        {
            _action.Invoke();
            yield break;
        }
    }
}