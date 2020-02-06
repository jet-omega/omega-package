using System;
using System.Collections;

namespace Omega.Routines
{
    public sealed class WaitRoutine<TResult> : Routine<TResult>, IProgressRoutineProvider
    {
        private Routine _sourceRoutine;
        private Func<TResult> _resultProvider;

        public WaitRoutine(Routine sourceRoutine, Func<TResult> resultProvider)
        {
            _sourceRoutine = sourceRoutine;
            _resultProvider = resultProvider;
        }

        protected override IEnumerator RoutineUpdate()
        {
            yield return _sourceRoutine;

            if (_sourceRoutine.IsError)
                throw _sourceRoutine.Exception;

            var result = _resultProvider();
            SetResult(result);
        }

        public float GetProgress()
        {
            return RoutineUtilities.GetProgressFromRoutine(_sourceRoutine);
        }
    }
}