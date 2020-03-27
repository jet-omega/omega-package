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
        [CanBeNull] private Action _update;
        [NotNull] private Action<Exception, Routine> _exceptionHandler = DefaultExceptionHandler;

        [CanBeNull] private string _creationStackTrace;

        public bool IsError => _status == RoutineStatus.Error;
        public bool IsProcessing => _status == RoutineStatus.Processing || _status == RoutineStatus.ForcedProcessing;
        public bool IsComplete => _status == RoutineStatus.Completed;
        public bool IsNotStarted => _status == RoutineStatus.NotStarted;
        public bool IsCanceled => _status == RoutineStatus.Canceled;

        protected bool IsForcedProcessing => _status == RoutineStatus.ForcedProcessing;

        public Exception Exception => _exception;

        protected abstract IEnumerator RoutineUpdate();

        private void SetupCompleted()
        {
            if (_status == RoutineStatus.Canceled)
                throw new InvalidOperationException("Routine was canceled");

            _status = RoutineStatus.Completed;
            _update?.Invoke();
            _callback?.Invoke();
        }

        bool IEnumerator.MoveNext()
        {
            // Если рутина содержит ошибку, то последующие ее выполнение может быть не корректным.
            if (IsError || IsCanceled || IsComplete)
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

                if (_status != RoutineStatus.ForcedProcessing)
                {
                    _status = RoutineStatus.Processing;
                    _update?.Invoke();
                }
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
                _update?.Invoke();

                return false;
            }

            // Если больше не можем двигаться дольше то помечаем рутину как завершенную  
            if (moveNextResult)
                _update?.Invoke();
            else
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
            {
                if (current is Routine nestedRoutine)
                {
                    var nestedRoutineStatus = nestedRoutine._status;

                    // if(nestedRoutineStatus == RoutineStatus.Error)
                    //     throw new Exception("Nested routine have error", nestedRoutine._exception);
                    // if(nestedRoutineStatus == RoutineStatus.Canceled)
                    //     throw new Exception("Nested routine were canceled");

                    if (_status == RoutineStatus.ForcedProcessing &&
                        nestedRoutineStatus != RoutineStatus.ForcedProcessing)
                        nestedRoutine.OnForcedCompleteInternal();
                }

                return DeepMoveNext(nestedEnumerator) || enumerator.MoveNext();
            }

            // Если текущее состояние рутины ожидает завершения асинхронной операции, то просто ждем ее завершения
            if (current is AsyncOperation nestedAsyncOperation)
                return !nestedAsyncOperation.isDone || enumerator.MoveNext();

            return enumerator.MoveNext();
        }

        internal void OnForcedCompleteInternal()
        {
            if (_status == RoutineStatus.Completed || _status == RoutineStatus.ForcedProcessing)
                return;

            if (_status is RoutineStatus.Error)
                throw new InvalidOperationException(
                    "Impossible to force complete a routine in which there is an error");

            _status = RoutineStatus.ForcedProcessing;

            OnForcedComplete();

            _update?.Invoke();
        }

        public void Cancel()
        {
            if (_status == RoutineStatus.Canceled || _status == RoutineStatus.Completed ||
                _status == RoutineStatus.Error)
                return;

            _status = RoutineStatus.Canceled;

            OnCancel();

            _update?.Invoke();
        }

        protected virtual void OnForcedComplete()
        {
        }

        protected virtual void OnCancel()
        {
        }

        // TODO: mb throw not supported exception?
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
            => _creationStackTrace = stackTrace;

        internal void SetExceptionHandlerInternal(Action<Exception, Routine> exceptionHandler)
            => _exceptionHandler = exceptionHandler;

        internal string GetCreationStackTraceInternal()
            => _creationStackTrace;

        internal void AddUpdateActionInternal(Action action)
            => _update += action;


        private enum RoutineStatus
        {
            NotStarted = 0,
            Processing,
            ForcedProcessing,
            Error,
            Completed,
            Canceled
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