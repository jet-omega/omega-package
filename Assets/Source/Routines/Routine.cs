using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using Omega.Package;
using UnityEngine;
using Debug = UnityEngine.Debug;
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
        [CanBeNull] private IEnumerator _routine;
        [CanBeNull] private Action _callback;
        [CanBeNull] private Action _update;
        [NotNull] private Action<Exception, Routine> _exceptionHandler = DefaultExceptionHandler;

        [CanBeNull] private string _creationStackTrace;

        public string Name { get; set; }

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

            if (_status == RoutineStatus.Error)
                throw new InvalidOperationException("Routine have error");

            _status = RoutineStatus.Completed;
            _update?.Invoke();
            _callback?.Invoke();
        }

        private IEnumerator GetStateMachine()
        {
            if (_routine == null)
                _routine = RoutineUpdate();

            return _routine;
        }


        internal static bool Execute2(Routine routine)
        {
            var stack = new Stack<Frame>();

            void stack_backward()
            {
                while (stack.Count > 0) stack.Pop().Context._update?.Invoke();
            }

            {
                var routineStateMachine = routine.GetStateMachine();
                stack.Push(new Frame(routine, routineStateMachine));
            }

            while (stack.Count > 0)
            {
                var (context, stateMachine) = stack.Peek();

                if (context.IsError || context.IsCanceled || context.IsComplete || stateMachine == null) // todo: mb remove? 
                    stack.Pop();
                // else if (stateMachine == null)
                // {
                //     context.SetupCompleted();
                //     stack.Pop();
                // }
                else
                {
                    var current = stateMachine.Current;

                    switch (current)
                    {
                        case Routine currentRoutine:
                            if (currentRoutine.IsError || currentRoutine.IsCanceled || currentRoutine.IsComplete)
                                break;
                            else
                            {
                                if (context.IsForcedProcessing)
                                {
                                    currentRoutine.OnForcedCompleteInternal();
                                }

                                if (currentRoutine.GetStateMachine() is null)
                                {
                                    currentRoutine.SetupCompleted();
                                    stack_backward();
                                    return true;
                                }

                                stack.Push(new Frame(currentRoutine, currentRoutine.GetStateMachine()));
                                continue;
                            }


                        case AsyncOperation currentAsyncOperation:
                            if (!currentAsyncOperation.CanBeForceComplete())
                            {
                                context._exception = new Exception("async operation exception. todo this message");
                                context._status = RoutineStatus.Error;
                                context._exceptionHandler?.Invoke(context._exception, context);

                                stack_backward();
                                return true;
                            }

                            stack_backward();
                            return !currentAsyncOperation.isDone;

                        case IEnumerator nestedEnumerator:
                            stack.Push(new Frame(context, nestedEnumerator));
                            continue;
                    }

                    bool? isMoveNext = null;
                    try
                    {
                        isMoveNext = stateMachine.MoveNext();
                    }
                    catch (Exception e)
                    {
                        context._exception = e;
                        context._status = RoutineStatus.Error;
                        context._exceptionHandler?.Invoke(context._exception, context);
                    }

                    if (isMoveNext.HasValue)
                        if (!isMoveNext.Value)
                            if (context.GetStateMachine() == stateMachine)
                                context.SetupCompleted();

                    stack_backward();
                    return true;
                }
            }

            return false;
        }

        private readonly struct Frame
        {
            public readonly Routine Context;
            public readonly IEnumerator StateMachine;

            public Frame(Routine context, IEnumerator stateMachine)
            {
                Context = context;
                StateMachine = stateMachine;
            }

            public void Deconstruct(out Routine context, out IEnumerator stateMachine)
            {
                context = Context;
                stateMachine = StateMachine;
            }
        }


        bool IEnumerator.MoveNext()
        {
            //todo: optimize condition. mb something typo `_status < 16` 
            // Если рутина содержит ошибку, то последующие ее выполнение может быть не корректным.
            // todo: execution round rework
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

            var moveNextResult = Execute2(this);

            // Для поддержки правильного состояния рутины изолируем исполнение пользовательского кода 
            // try
            // {
            //     moveNextResult = DeepMoveNext(_routine);
            // }
            // catch (Exception e)
            // {
            //     // В случае если пользовательский код вызвал исключение, то обновляем состояние рутины
            //     // и обрабатываем это исключение
            //     _exception = e;
            //     _status = RoutineStatus.Error;
            //
            //     _exceptionHandler.Invoke(e, this);
            //     _update?.Invoke();
            //
            //     return false;
            // }

            // Если больше не можем двигаться дольше то помечаем рутину как завершенную  
            // if (moveNextResult)
            //     _update?.Invoke();
            // else

            // if (!moveNextResult && !IsCanceled)
            //     SetupCompleted();

            return moveNextResult;
        }

        private static bool Execute(Routine routine)
        {
            var stack = new Stack<IEnumerator>(3);
            stack.Push(routine);

            void Backward()
            {
                while (stack.Count > 0)
                {
                    var current = stack.Pop();
                    if (current is Routine currentRoutine)
                        currentRoutine._update?.Invoke();
                }
            }

            bool isForced = false;

            while (stack.Count > 0)
            {
                var current = stack.Peek();

                if (current is Routine currentRoutine)
                {
                    if (currentRoutine.IsError || currentRoutine.IsCanceled || currentRoutine.IsComplete)
                    {
                        isForced = false;
                        stack.Pop();
                        continue;
                    }

                    var routineStateMachine = currentRoutine.GetStateMachine();
                    if (routineStateMachine != null)
                        stack.Push(routineStateMachine);
                    else
                    {
                        currentRoutine.SetupCompleted();
                        Backward();
                        return true;
                    }

                    if (currentRoutine.IsForcedProcessing)
                        isForced = true;

                    continue;
                }

                var nested = current.Current;

                if (nested is IEnumerator nestedEnumerator)
                {
                    if (nested is Routine nestedRoutine)
                    {
                        if (!nestedRoutine.IsError && !nestedRoutine.IsCanceled && !nestedRoutine.IsComplete)
                        {
                            stack.Push(nestedEnumerator);
                            if (isForced)
                                nestedRoutine.OnForcedCompleteInternal();

                            continue;
                        }
                    }
                    else
                    {
                        stack.Push(nestedEnumerator);
                        continue;
                    }
                }
                else if (nested is AsyncOperation nestedAsyncOperation)
                {
                    if (isForced && !nestedAsyncOperation.CanBeForceComplete())
                        throw new Exception("error");

                    if (!nestedAsyncOperation.isDone)
                    {
                        Backward();
                        return true;
                    }
                }

                if (current.MoveNext()) // this move next may throw exception
                {
                    Backward();
                    return true;
                }

                stack.Pop();
                if (stack.Peek() is Routine completedRoutine)
                    completedRoutine.SetupCompleted();
            }

            return false;
        }

        /// <summary>
        /// Обновляет состояние рутины со всеми вложениями
        /// </summary>
        /// <param name="enumerator"></param>
        /// <returns></returns>
        private bool DeepMoveNext(IEnumerator enumerator)
        {
            bool ContinuePath(IEnumerator c)
            {
                return true;
            }

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

                    if (nestedRoutineStatus == RoutineStatus.Error)
                        throw new Exception("Nested routine have error", nestedRoutine._exception);
                    if (nestedRoutineStatus == RoutineStatus.Canceled)
                        throw new Exception("Nested routine were canceled");

                    var isProcessingNestedRoutine = nestedRoutineStatus != RoutineStatus.Canceled
                                                    && nestedRoutineStatus != RoutineStatus.Completed
                                                    && nestedRoutineStatus != RoutineStatus.Error;

                    if (isProcessingNestedRoutine &&
                        _status == RoutineStatus.ForcedProcessing &&
                        nestedRoutineStatus != RoutineStatus.ForcedProcessing)
                        nestedRoutine.OnForcedCompleteInternal();

                    if (nestedRoutine.DeepMoveNext(nestedEnumerator))
                        return true;
                }
            }

            // Если текущее состояние рутины ожидает завершения асинхронной операции, то просто ждем ее завершения
            else if (current is AsyncOperation nestedAsyncOperation)
            {
                if (_status == RoutineStatus.ForcedProcessing && !nestedAsyncOperation.CanBeForceComplete())
                    throw new InvalidOperationException("You cant force complete that async-operation: " +
                                                        nestedAsyncOperation.GetType());

                if (!nestedAsyncOperation.isDone)
                    return true;
            }

            return !IsError && !IsCanceled && !IsComplete && enumerator.MoveNext();
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