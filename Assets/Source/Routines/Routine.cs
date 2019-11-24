using System;
using System.Collections;
using System.Diagnostics;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Omega.Package;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Omega.Routines
{
    public abstract partial class Routine : IEnumerator
    {
        public static readonly Action<Exception, Routine> DefaultExceptionHandler
            = delegate(Exception exception, Routine routine)
            {
                var message = ExceptionHelper.Messages.CreateExceptionMessageForRoutine(routine, exception);
                Debug.LogError(message);
            };

        private RoutineStatus _status;
        [CanBeNull] private Exception _exception;
        [CanBeNull] private IEnumerator _routine;
        [CanBeNull] private Action _callback;
        [NotNull] private Action<Exception, Routine> _exceptionHandler = DefaultExceptionHandler;

        [CanBeNull] private string _creationStackTrace;

        public bool IsError => _status == RoutineStatus.Error;
        public bool IsProcessing => _status == RoutineStatus.Processing;
        public bool IsComplete => _status == RoutineStatus.Completed;
        public bool IsNotStarted => _status == RoutineStatus.NotStarted;

        public Exception Exception => _exception;

        protected abstract IEnumerator RoutineUpdate();

        private void SetupCompleted()
        {
            _status = RoutineStatus.Completed;
            _callback?.Invoke();
        }

        bool IEnumerator.MoveNext()
        {
            // Если рутина содержит ошибку, то последующие ее выполнение может быть не корректным.
            if (IsError)
                return false;

            // Если рутина еще не создана - создаем
            if (_routine == null)
            {
                _routine = RoutineUpdate();
                if (_routine == null)
                {
                    SetupCompleted();
                    return false;
                }

                _status = RoutineStatus.Processing;
            }

            bool moveNextResult = false;

            // Для поддержки правильного состояния рутины изолируем исполнение пользовательского кода 
            try
            {
                moveNextResult = DeepMoveNext(_routine);
            }
            catch (Exception e)
            {
                // В случае если пользовательский код вызвал исключение, то обновляем состояние рутины
                // и обрабатываем это исключение
                _exception = e;
                _status = RoutineStatus.Error;

                _exceptionHandler.Invoke(e, this);

                return false;
            }

            // Если больше не можем двигаться дольше то помечаем рутину как завершенную  
            if (!moveNextResult)
                SetupCompleted();

            return moveNextResult;
        }

        /// <summary>
        /// Обновляет состояние рутины со всеми вложениями
        /// </summary>
        /// <param name="enumerator"></param>
        /// <returns></returns>
        private bool DeepMoveNext(IEnumerator enumerator)
        {
            // Пытаемся получить состояние рутины 
            var current = enumerator.Current;

            // Если текущее состояние рутины ожидает завершения другой рутины
            if (current is IEnumerator nestedEnumerator)
                // То ожидаем эту вложенную рутину со всеми ее вложениями
                // Если обновить состояние рутины не удалось то двигаем рутину которая содержала в себе вложенную рутину
                return DeepMoveNext(nestedEnumerator) || enumerator.MoveNext();

            // Если текущее состояние рутины ожидает завершения асинхронной операции, то просто ждем ее завершения
            if (current is AsyncOperation nestedAsyncOperation)
                return !nestedAsyncOperation.isDone;

            return enumerator.MoveNext();
        }

        void IEnumerator.Reset()
        {
            _exceptionHandler = DefaultExceptionHandler;
            _status = RoutineStatus.NotStarted;
            _routine = null;
            _exception = null;
            _callback = null;
        }

        object IEnumerator.Current => _routine?.Current;

        internal void AddCallbackInternal(Action callback)
            => _callback += callback;

        internal void SetCreationStackTraceInternal(string stackTrace)
        {
            _creationStackTrace = stackTrace;
        }

        internal void SetExceptionHandlerInternal(Action<Exception, Routine> exceptionHandler)
            => _exceptionHandler = exceptionHandler;

        internal string GetCreationStackTraceInternal()
            => _creationStackTrace;

        private enum RoutineStatus
        {
            NotStarted = 0,
            Processing,
            Error,
            Completed
        }

        public static implicit operator bool([CanBeNull] Routine routine)
            => routine == null || !routine.IsProcessing && !routine.IsNotStarted;

        [NotNull]
        public static Routine operator +([NotNull] Routine lhs, [NotNull] Routine rhs)
        {
            if (rhs == null)
                throw new ArgumentNullException(nameof(rhs));
            if (lhs == null)
                throw new ArgumentNullException(nameof(lhs));

            var lhsConcatenation = lhs as ConcatenationRoutine;
            var rhsConcatenation = rhs as ConcatenationRoutine;

            if (lhsConcatenation is null && rhsConcatenation is null)
                return new ConcatenationRoutine(lhs, rhs);

            if (lhsConcatenation is null)
                return rhsConcatenation.Add(lhs);

            if (rhsConcatenation is null)
                return lhsConcatenation.Add(rhs);

            return lhsConcatenation.Add(rhsConcatenation);
        }
    }
}