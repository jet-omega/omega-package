using System;
using System.Collections;
using UnityEngine;

namespace Omega.Routines
{
    internal sealed class ProgressRoutine : Routine, IProgressRoutineProvider
    {
        private Routine _targetRoutine;
        private RoutineProgressHandler _routineProgress;
        private Action<float> _progressHandler;

        public ProgressRoutine(Routine targetRoutine, Action<float> progressHandler)
        {
            _targetRoutine = targetRoutine;
            _routineProgress = new RoutineProgressHandler(targetRoutine);
            _progressHandler = progressHandler;
        }

        protected override IEnumerator RoutineUpdate()
        {
            while (true)
            {
                if (_routineProgress.TryUpdateProgress(out var progress))
                    _progressHandler.Invoke(progress);

                if (_targetRoutine.IsComplete || _targetRoutine.IsError)
                    yield break;

                yield return null;
            }
        }

        public float GetProgress()
        {
            return _routineProgress.Progress;
        }
    }
}