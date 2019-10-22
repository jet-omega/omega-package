using System;
using System.Collections;
using UnityEngine;

namespace Omega.Experimental.Routines
{
    public abstract class Routine : IEnumerator
    {
        private RoutineStatus _status;
        private Exception _exception;
        private IEnumerator _routine;

        public bool IsError => _status.HasFlag(RoutineStatus.Error);

        public Exception Exception => _exception;
        
        protected void SetException(Exception exception)
        {
            _status = RoutineStatus.Error;
            _exception = exception;
        }

        protected virtual void SetComplete()
        {
            _status = RoutineStatus.Completed;
        }

        protected abstract IEnumerator RoutineUpdate();

        protected enum RoutineStatus
        {
            None = 0,
            Processing,
            Error,
            Canceled,
            Completed
        }

        public Routine GetRoutine(out Routine routine) => routine = this;

        bool IEnumerator.MoveNext()
        {
            if (_routine == null)
                _routine = RoutineUpdate();

            return _routine.MoveNext();
        }

        void IEnumerator.Reset()
        {
            throw new NotSupportedException();
            _routine = null;
        }

        object IEnumerator.Current => (_routine ?? throw new Exception()).Current;

        public static Routine Task(Action task) => new OtherThreadRoutine(task);
        public static Routine<TResult> Task<TResult>(Func<TResult> task) => new OtherThreadRoutine<TResult>(task); 
    }
}