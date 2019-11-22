using System;
using System.Collections;

namespace Omega.Routines
{
    internal sealed class ProgressRoutine : Routine, IProgressRoutineProvider
    {
        private Routine _targetRoutine;
        private RoutineProgress _routineProgress;
        private Action<float> _progressHandler;

        public ProgressRoutine(Routine targetRoutine, Action<float> progressHandler)
        {
            _targetRoutine = targetRoutine;
            _routineProgress = new RoutineProgress(targetRoutine);
            _progressHandler = progressHandler;
        }

        protected override IEnumerator RoutineUpdate()
        {
            while (!_targetRoutine.IsComplete && !_targetRoutine.IsError)
            {
                if (_routineProgress.TryUpdateProgress(out var progress))
                    _progressHandler.Invoke(progress);
                
                yield return null;
            }
        }

        public float GetProgress()
        {
            return 1f;
        }
    }
}