using System;
using System.Collections;

namespace Omega.Routines
{
    internal sealed class ConvertResultRoutine<TInitialResult, TTargetResult> : Routine<TTargetResult>
    {
        private Routine<TInitialResult> _routine;
        private Func<TInitialResult, TTargetResult> _converter;

        public ConvertResultRoutine(Routine<TInitialResult> routine, Func<TInitialResult, TTargetResult> converter)
        {
            _routine = routine;
            _converter = converter;
        }

        protected override IEnumerator RoutineUpdate()
        {
            yield return _routine;

            var initialResult = _routine.GetResult();
            var targetResult = _converter(initialResult);
            
            SetResult(targetResult);
        }
    }
}