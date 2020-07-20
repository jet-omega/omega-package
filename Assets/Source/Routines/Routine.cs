using System;
using System.Collections;
using System.Text;
using JetBrains.Annotations;
using Omega.Package;
using Omega.Routines.Exceptions;
using UnityEngine;
using Logger = Omega.Package.Logger;

namespace Omega.Routines
{
    public abstract partial class Routine : IEnumerator
    {
        internal static readonly Logger Logger = new Logger("ROUTINE▶", new Color32(0xFF, 0xA5, 0x00, 0xFF),
            FontStyle.Bold);

        public static readonly Action<Exception, Routine> DefaultExceptionHandler
            = delegate(Exception exception, Routine routine)
            {
                var message = ExceptionHelper.Messages.CreateExceptionMessageForRoutine(routine, exception);
                Logger.Log(message, LogType.Error);
            };

        private RoutineStatus _status;
        [CanBeNull] private Exception _exception;
        [CanBeNull] private IEnumerator _routineStateMachine;
        [CanBeNull] private Action _updateRoutine;
        [CanBeNull] private Action _anyCaseCallback;
        [NotNull] private Action<Exception, Routine> _exceptionHandler = DefaultExceptionHandler;

        [CanBeNull] private string _creationStackTrace;

        [CanBeNull] public string Name { get; set; }

        public bool IsError => _status == RoutineStatus.Error;
        public bool IsProcessing => _status == RoutineStatus.Processing || _status == RoutineStatus.ForcedProcessing;
        public bool IsComplete => _status == RoutineStatus.Completed;
        public bool IsNotStarted => _status == RoutineStatus.NotStarted;
        public bool IsCanceled => _status == RoutineStatus.Canceled;

        protected internal bool IsForcedProcessing => _status == RoutineStatus.ForcedProcessing;

        [CanBeNull] public Exception Exception => _exception;

        protected abstract IEnumerator RoutineUpdate();

        internal IEnumerator GetStateMachine()
        {
            if (_routineStateMachine == null)
                _routineStateMachine = RoutineUpdate();

            return _routineStateMachine;
        }

        internal void UpdatePulse()
        {
            _updateRoutine?.Invoke();
        }
        
        bool IEnumerator.MoveNext()
        {
            // todo: optimize condition. mb something typo `_status < 16` 
            // Если рутина содержит ошибку, то последующие ее выполнение может быть не корректным.
            // todo: execution round rework
            if (IsError || IsCanceled || IsComplete)
                return false;

            if (GetStateMachine() is null)
            {
                SetupCompletedInternal();
                return false;
            }

            if (_status != RoutineStatus.ForcedProcessing)
            {
                _status = RoutineStatus.Processing;
                _updateRoutine?.Invoke();
            }

            return RoutineExecution.ExecuteRound(this);
        }

        public void Cancel()
        {
            if (_status == RoutineStatus.Canceled || _status == RoutineStatus.Completed ||
                _status == RoutineStatus.Error)
                return;

            _status = RoutineStatus.Canceled;

            OnCancel();

            _anyCaseCallback?.Invoke();
            _updateRoutine?.Invoke();
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
            _routineStateMachine = null;
            _exception = null;
            _anyCaseCallback = null;
        }

        // Routine отличается от корутины Unity тем что рутина выполняется самостоятельно, то есть, 
        // всю рутину можно полностью выполнить вызовами MoveNext при этом все вложенные рутины также будут выполнены
        // в случае с корутинами Unity все немного сложнее, у вас нет гарантий что ваши вызовы MoveNext не сломают
        // вложенность корутины, так как все вложенные корутины, а так же вложенные асинхронные операции
        // решает сама Unity (внутри StartCoroutine)
        //
        // Допустим у нас есть такой Enumerator:
        //
        // IEnumerator Enumerator()
        // {
        //     // Ждем когда пройдет 5 секунд 
        //     yield return new WaitForSeconds(5);
        //     Debug.Log("Complete!")
        // }
        //
        // Если мы будем использовать этот Enumerator как корутину Unity 
        // и сделаем вызов StartCoroutine(Enumerator()) то как и ожидается, через 5 секунд будет залоггировано "Complete!" 
        // Однако если мы уберем знание о том что это корутина и сделаем что-то такое: 
        // 
        // var enumerator = Enumerator();
        // while(enumerator.MoveNext())
        // { }
        //
        // То тогда мы также увидим сообщение "Complete!", однако 5 секунд не пройдет, так как никто их не подождал.  
        // то есть в цикле приведенном выше будет всего одна итерация (так как у нас один yield return внутри Enumerator) 
        // 
        // Попробуем сделать то же самое с помощью рутин (Omega.Routine) 
        // 
        // var routine = Routine.ByEnumerator(Enumerator());
        // var enumerator = routine as IEnumerator;
        // while(enumerator.MoveNext())
        // { }
        // 
        // Теперь мы получим задержку в заветные 5 секунд и только после этого увидим сообщение
        // 
        // Unity внутри StartCoroutine сама обрабатывает все вложенные IEnumerator-ы и мы никак не можем на это повлиять   
        // поэтому IEnumerator.Current должна всегда возвращать null, чтобы Unity всегда обрабатывала верхнюю рутину а не внутреннею  
        object IEnumerator.Current => null;

        internal void AddCallbackInternal(Action callback)
            => _anyCaseCallback += callback;

        internal void SetCreationStackTraceInternal(string stackTrace)
            => _creationStackTrace = stackTrace;

        internal void SetExceptionHandlerInternal(Action<Exception, Routine> exceptionHandler)
            => _exceptionHandler = exceptionHandler;

        internal string GetCreationStackTraceInternal()
            => _creationStackTrace;

        internal void AddUpdateActionInternal(Action action)
            => _updateRoutine += action;

        internal RoutineStatus GetStatus() => _status;

        internal void SetupProgressingInternal()
            => _status = RoutineStatus.Processing;

        internal void SetupForcedProcessingInternal()
        {
            if (_status == RoutineStatus.Completed || _status == RoutineStatus.ForcedProcessing)
                return;

            if (_status is RoutineStatus.Error)
                throw new RoutineErrorException("routine cant be (forced) processing because it have error");

            _status = RoutineStatus.ForcedProcessing;

            OnForcedComplete();

            _updateRoutine?.Invoke();
        }
        
        internal void SetupCompletedInternal()
        {
            if(_status == RoutineStatus.Completed)
                return;
            
            if (_status == RoutineStatus.Canceled)
                throw new InvalidOperationException("routine cant be completed because it was cancelled");

            if (_status == RoutineStatus.Error)
                throw new InvalidOperationException("routine cant be completed because it have error");

            _status = RoutineStatus.Completed;
            _updateRoutine?.Invoke();
            _anyCaseCallback?.Invoke();
        }
        
        internal void SetupErrorInternal(Exception exception)
        {
            if(_status == RoutineStatus.Error)
                throw new RoutineErrorException("it is not possible to set an error to a routine because it already has an error");
            
            _exception = exception;
            _status = RoutineStatus.Error;
            _anyCaseCallback?.Invoke();
            _exceptionHandler.Invoke(exception, this);
        }

        internal enum RoutineStatus
        {
            NotStarted = 0,
            Processing,
            ForcedProcessing,
            Error,
            Completed,
            Canceled
        }

        [Obsolete]
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

        public override string ToString()
        {
            var sb = new StringBuilder(Name?.Length ?? 0 + 50);

            sb.Append($"{GetType().Name} {{");

            if (!string.IsNullOrEmpty(Name))
                sb.Append($"Name: {Name}, ");

            sb.Append($"Status: {_status} ");
            if (IsProcessing && this is IProgressRoutineProvider progressProvider)
                sb.Append($"({progressProvider.GetProgress():P}) ");

            sb.Append('}');

            return sb.ToString();
        }
    }
}