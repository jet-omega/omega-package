using System;
using System.Collections;

namespace Omega.Routines
{
    public class InvokeWhileRoutine : Routine
    {
        private readonly Action _updateAction;
        private readonly Routine _targetRoutine;
        
        internal InvokeWhileRoutine(Routine targetTargetRoutine, Action action)
        {
            _targetRoutine = targetTargetRoutine;
            _updateAction = action;
        }

        protected override IEnumerator RoutineUpdate()
        {
            yield return _targetRoutine;
            _updateAction.Invoke();
        }
    }
}