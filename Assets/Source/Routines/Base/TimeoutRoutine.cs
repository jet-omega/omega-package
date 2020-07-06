using System;
using System.Collections;

namespace Omega.Routines
{
    public sealed class TimeoutRoutine : Routine, IProgressRoutineProvider
    {
        private readonly Routine _sourceRoutine;
        private readonly TimeSpan _timeout;

        private DateTime _timeoutTime;

        internal TimeoutRoutine(Routine sourceRoutine, TimeSpan timeout)
        {
            _timeout = timeout;
            _sourceRoutine = sourceRoutine;
        }

        protected override IEnumerator RoutineUpdate()
        {
            _timeoutTime = DateTime.Now + _timeout;

            while (((IEnumerator) _sourceRoutine).MoveNext())
            {
                if (DateTime.Now > _timeoutTime)
                    throw new TimeoutException();
                yield return null;
            }
        }

        public float GetProgress() => RoutineUtilities.GetProgressFromRoutine(_sourceRoutine);
    }
}