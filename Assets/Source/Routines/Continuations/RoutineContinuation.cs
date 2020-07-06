using System;
using System.Collections;
using JetBrains.Annotations;

namespace Omega.Routines
{
    public sealed class RoutineContinuation : Routine, IRoutineContinuation
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

        bool IRoutineContinuation.TryContinue(out Exception continuationException)
        {
            if (_routine.IsError && !_continuationCase.HasFlag(CompletionCase.Error))
            {
                continuationException = new Exception("cant continue because routine have error");
                return false;
            }

            if (_routine.IsCanceled && !_continuationCase.HasFlag(CompletionCase.Canceled))
            {
                continuationException = new Exception("cant continue because routine is cancelled");
                return false;
            }

            continuationException = null;
            return true;
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