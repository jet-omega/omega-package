using System;
using System.Diagnostics;
using JetBrains.Annotations;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Omega.Routines
{
    public static class RoutineExtension
    {
        [Obsolete("Use OnProgress")]
        public static Routine OnChangeProgress(this Routine self, Action<float> handler)
        {
            Routine.Logger.Log("OnChangeProgress is deprecated, use OnProgress", LogType.Error);

            if (self is null)
                throw new NullReferenceException(nameof(self));
            if (handler is null)
                throw new ArgumentNullException(nameof(handler));

            var progressRoutine = new ProgressRoutine(self, handler);
            var groupRoutine = new GroupRoutine(self, progressRoutine);
            return groupRoutine;
        }

        public static TRoutine OnProgress<TRoutine>(this TRoutine self, Action<float> handler)
            where TRoutine : Routine
        {
            if (self is null)
                throw new NullReferenceException(nameof(self));
            if (handler is null)
                throw new ArgumentNullException(nameof(handler));

            var p = new RoutineProgressHandler(self);
            self.AddUpdateActionInternal(() =>
            {
                if (p.TryUpdateProgress(out var progress))
                    handler.Invoke(progress);
            });

            return self;
        }

        [Obsolete("Use Self")]
        public static TRoutine GetSelf<TRoutine>(this TRoutine self, out TRoutine routine)
            where TRoutine : Routine
        {
            if (self == null)
                throw new NullReferenceException(nameof(self));

            return routine = self;
        }

        public static TRoutine Self<TRoutine>(this TRoutine self, out TRoutine routine)
            where TRoutine : Routine
        {
            if (self == null)
                throw new NullReferenceException(nameof(self));

            return routine = self;
        }

        public static TRoutine SetName<TRoutine>(this TRoutine self, [NotNull] string name)
            where TRoutine : Routine
        {
            if (self == null)
                throw new NullReferenceException(nameof(self));

            self.Name = name;

            return self;
        }

        public static TRoutine Callback<TRoutine>(this TRoutine original, Action callback)
            where TRoutine : Routine
        {
            if (original == null)
                throw new NullReferenceException(nameof(original));
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            if (original.IsComplete)
                throw new InvalidOperationException("callback will never call because routine is completed");
            if (original.IsError)
                throw new InvalidOperationException("callback will never call because routine have error");

            original.AddCallbackInternal(callback);
            return original;
        }

        public static Routine<TResult> Callback<TResult>(this Routine<TResult> original,
            Action<TResult> callback)
        {
            if (original == null)
                throw new NullReferenceException(nameof(original));
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            if (original.IsComplete)
                throw new InvalidOperationException("callback will never call because routine is completed");
            if (original.IsError)
                throw new InvalidOperationException("callback will never call because routine have error");

            original.AddCallbackInternal(() => callback.Invoke(original.GetResult()));
            return original;
        }

        public static Routine Catch(this Routine routine, CompletionCase completionCase, bool withSuccess = true)
        {
            var finalCompletionCase = withSuccess
                ? CompletionCase.Success | completionCase
                : completionCase;

            return new RoutineContinuation(routine, finalCompletionCase);
        }

        public static Routine<TResult> Result<TResult>(this Routine<TResult> original,
            out Routine<TResult>.ResultContainer result)
        {
            if (original == null)
                throw new NullReferenceException(nameof(original));

            result = new Routine<TResult>.ResultContainer(original);
            return original;
        }

        public static void Complete(this Routine original)
        {
            if (original == null)
                throw new NullReferenceException(nameof(original));

            RoutineUtilities.CompleteWithoutChecks(original);
        }

        public static void Complete(this Routine original, TimeSpan timeout)
        {
            if (original == null)
                throw new NullReferenceException(nameof(original));

            var timeoutDuration = timeout.Duration();

            RoutineUtilities.CompleteWithoutChecks(original, timeoutDuration);
        }

        public static void Complete(this Routine original, float timeoutSeconds)
        {
            if (original == null)
                throw new NullReferenceException(nameof(original));

            var timeoutAbs = Mathf.Abs(timeoutSeconds);
            var timeoutTimeSpan = TimeSpan.FromSeconds(timeoutAbs);

            RoutineUtilities.CompleteWithoutChecks(original, timeoutTimeSpan);
        }

        public static TRoutine ExceptionHandler<TRoutine>(this TRoutine original, Action<Exception> exceptionHandler)
            where TRoutine : Routine
            => ExceptionHandler(original, (e, r) => exceptionHandler(e));

        public static TRoutine ExceptionHandler<TRoutine>(this TRoutine original,
            Action<Exception, Routine> exceptionHandler)
            where TRoutine : Routine
        {
            if (original == null)
                throw new NullReferenceException(nameof(original));
            if (exceptionHandler == null)
                throw new ArgumentNullException(nameof(exceptionHandler));

            original.SetExceptionHandlerInternal(exceptionHandler);

            return original;
        }

        public static TRoutine CreationStackTrace<TRoutine>(this TRoutine original)
            where TRoutine : Routine
        {
            if (original == null)
                throw new NullReferenceException(nameof(original));
            if (!original.IsNotStarted)
                throw new AggregateException();

            var extracted = Package.StackTraceUtility.ExtractFormattedStackTrace(new StackTrace(1, true));

            original.SetCreationStackTraceInternal(extracted);

            return original;
        }

        /// <summary>
        /// Запускает и обрабатывает рутину в фоне
        /// </summary>
        /// <param name="self"></param>
        /// <param name="executionOrder">Определяет в какой момент обрабатывается рутина</param>
        /// <param name="scope">Определяет время жизни обработки рутины</param>
        /// <param name="prelude">true - если перед помещением рутины в worker необходимо проиграть рутину до первого yield, иначе - false</param>
        /// <returns></returns>
        public static RoutineExecutionHandler InBackground(this Routine self,
            ExecutionOrder executionOrder = ExecutionOrder.Update,
            RoutineExecutionScope scope = RoutineExecutionScope.Scene,
            bool prelude = true)
            => RoutineWorkerHub.Add(self, scope, executionOrder, prelude);

        public static RoutineExecutionHandler InBackground(this Routine self, bool prelude)
            => InBackground(self, ExecutionOrder.Update, RoutineExecutionScope.Scene, prelude);

        public static RoutineExecutionHandler InBackground(this Routine self, RoutineExecutionScope scope,
            bool prelude = true)
            => InBackground(self, ExecutionOrder.Update, scope, prelude);
    }
}