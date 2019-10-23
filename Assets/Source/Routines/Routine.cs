using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

namespace Omega.Routines
{
    public abstract class Routine : IEnumerator
    {
        public static readonly Action<Exception> DefaultExceptionHandler = Debug.LogException;

        private RoutineStatus _status;
        [CanBeNull] private Exception _exception;
        [CanBeNull] private IEnumerator _routine;
        [CanBeNull] private Action _callback;
        [NotNull] private Action<Exception> _exceptionHandler = DefaultExceptionHandler;

        public bool IsError => _status == RoutineStatus.Error;
        public bool IsProcessing => _status == RoutineStatus.Processing;
        public bool IsComplete => _status == RoutineStatus.Completed;
        
        public Exception Exception => IsError ? _exception : throw new Exception();

        protected abstract IEnumerator RoutineUpdate();

        bool IEnumerator.MoveNext()
        {
            // Если рутина содержит ошибку, то последующие ее выполнение может быть не корректным.
            if (IsError)
                return false;

            // Если рутина еще не создана - создаем
            if (_routine == null)
            {
                _routine = RoutineUpdate();
                _status = RoutineStatus.Processing;
            }

            bool moveNextResult = false;

            // Для поддержки правильного состояния рутины изолируем исполнение пользовательского кода 
            try
            {
                moveNextResult = _routine.MoveNext();
            }
            catch (Exception e)
            {
                // В случае если пользовательский код вызвал исключение, то обновляем состояние рутины
                // и обрабатываем это исключение
                _exception = e;
                _status = RoutineStatus.Error;
                
                _exceptionHandler.Invoke(e);

                return false;
            }

            // Если больше не можем двигаться дольше то помечаем рутину как завершенную  
            if (!moveNextResult)
            {
                _status = RoutineStatus.Completed;
                _callback?.Invoke();
            }

            return moveNextResult;
        }

        void IEnumerator.Reset()
        {
            _exceptionHandler = DefaultExceptionHandler;
            _status = RoutineStatus.ReadyToStart;
            _routine = null;
            _exception = null;
            _callback = null;
        }

        object IEnumerator.Current => (_routine ?? throw new Exception()).Current;

        internal void AddCallbackInternal(Action callback) 
            => _callback += callback;
        internal void SetExceptionHandlerInternal(Action<Exception> exceptionHandler) =>
            _exceptionHandler = exceptionHandler; 
        
        public static OtherThreadRoutine Task(Action task) => new OtherThreadRoutine(task);

        public static OtherThreadRoutine<TResult> Task<TResult>(Func<TResult> task) =>
            new OtherThreadRoutine<TResult>(task);
        
        private enum RoutineStatus
        {
            ReadyToStart = 0,
            Processing,
            Error,
            Completed
        }
    }
}