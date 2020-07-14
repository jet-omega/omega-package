using System;
using System.Collections;

namespace Omega.Routines
{
    public class InvokeWhileRoutine : Routine
    {
        private readonly Action _updateAction;

        private bool _isTargetRoutineRunning = true;
        
        internal InvokeWhileRoutine(Routine targetRoutine, Action action)
        {
            targetRoutine.Callback(() => _isTargetRoutineRunning = false);
            _updateAction = action;
        }
        
        protected override IEnumerator RoutineUpdate()
        {
            while (_isTargetRoutineRunning)
            {
                _updateAction.Invoke();
                yield return null;
            }
        }
    }
}