using System;
using System.Collections;
using JetBrains.Annotations;

namespace Omega.Routines
{
    public sealed class RoutineContinuation : Routine, IRoutineContinuation, IProgressRoutineProvider
    {
        [NotNull] private readonly Routine _routine;
        private readonly CompletionCase _continuationCase;

        public RoutineContinuation([NotNull] Routine routine, CompletionCase continuationCase)
        {
            _routine = routine ?? throw new ArgumentNullException(nameof(routine));
            _continuationCase = continuationCase;
        }

        protected override IEnumerator RoutineUpdate()
        {
            yield return _routine;
        }

        bool IRoutineContinuation.CanContinue()
        {
            if (_routine.IsError && !_continuationCase.HasFlag(CompletionCase.Error))
                return false;

            if (_routine.IsCanceled && !_continuationCase.HasFlag(CompletionCase.Canceled))
                return false;

            return true;
        }

        public float GetProgress()
        {
            return RoutineUtilities.GetProgressFromRoutine(_routine);
        }
    }

    [Flags]
    public enum CompletionCase
    {
        Success = 1,
        Canceled = 2,
        Error = 4,
        Any = Success | Canceled | Error,
    }
}