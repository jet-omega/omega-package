using System;
using System.Diagnostics;
using UnityEngine;

namespace Omega.Routines
{
    public static class RoutineExtension
    {
        [Obsolete("Use OnProgress")]
        public static Routine OnChangeProgress(this Routine self, Action<float> handler)
        {
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

        [Obsolete("Use GetSelf")]
        public static TRoutine GetRoutine<TRoutine>(this TRoutine self, out TRoutine routine)
            where TRoutine : Routine
            => GetSelf(self, out routine);

        public static TRoutine GetSelf<TRoutine>(this TRoutine self, out TRoutine routine)
            where TRoutine : Routine
        {
            if (self == null)
                throw new NullReferenceException(nameof(self));

            return routine = self;
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

        public static Routine<TResult> Result<TResult>(this Routine<TResult> original,
            out Routine<TResult>.ResultContainer result)
        {
            if (original == null)
                throw new NullReferenceException(nameof(original));

            result = new Routine<TResult>.ResultContainer(original);
            return original;
        }

        public static TRoutine Complete<TRoutine>(this TRoutine original)
            where TRoutine : Routine
        {
            if (original == null)
                throw new NullReferenceException(nameof(original));

            RoutineUtilities.CompleteWithoutChecks(original);
            return original;
        }

        public static TRoutine Complete<TRoutine>(this TRoutine original, TimeSpan timeout)
            where TRoutine : Routine
        {
            if (original == null)
                throw new NullReferenceException(nameof(original));

            var timeoutDuration = timeout.Duration();

            RoutineUtilities.CompleteWithoutChecks(original, timeoutDuration);
            return original;
        }

        public static TRoutine Complete<TRoutine>(this TRoutine original, float timeoutSeconds)
            where TRoutine : Routine
        {
            if (original == null)
                throw new NullReferenceException(nameof(original));

            var timeoutAbs = Mathf.Abs(timeoutSeconds);
            var timeoutTimeSpan = TimeSpan.FromSeconds(timeoutAbs);

            RoutineUtilities.CompleteWithoutChecks(original, timeoutTimeSpan);
            return original;
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

            var stackTrace = new StackTrace(1, true).ToString();

            original.SetCreationStackTraceInternal(stackTrace);

            return original;
        }

        /// <summary>
        /// Запускает и обрабатывает рутину в фоне
        /// </summary>
        /// <param name="self"></param>
        /// <param name="executionOrder">Определяет в какой момент обрабатывается рутина</param>
        /// <param name="scope">Определяет время жизни обработки рутины</param>
        /// <param name="prelude">true - если перед помещением рутины в worker необходимо проиграть рутину до первого yield, иначе - false</param>
        /// <typeparam name="TRoutine"></typeparam>
        /// <returns></returns>
        public static TRoutine InBackground<TRoutine>(this TRoutine self,
            ExecutionOrder executionOrder = ExecutionOrder.Update,
            RoutineExecutionScope scope = RoutineExecutionScope.Scene,
            bool prelude = true)
            where TRoutine : Routine
        {
            if (!prelude || RoutineUtilities.OneStep(self))
            {
                var worker = RoutineWorker.Instance;
                worker.Add(self, scope, executionOrder);
            }

            return self;
        }

        public static TRoutine InBackground<TRoutine>(this TRoutine self, bool prelude)
            where TRoutine : Routine
        {
            return InBackground(self, ExecutionOrder.Update, RoutineExecutionScope.Scene, prelude);
        }
    }
}