using System;
using System.Diagnostics;
using Omega.Tools.Experimental.UtilitiesAggregator;

namespace Omega.Routines
{
    public static class RoutineExtension
    {
        public static TRoutine GetRoutine<TRoutine>(this TRoutine original, out TRoutine routine)
            where TRoutine : Routine
        {
            if (original == null)
                throw new NullReferenceException(nameof(original));

            return routine = original;
        }

        public static TRoutine Callback<TRoutine>(this TRoutine original, Action callback)
            where TRoutine : Routine
        {
            if (original == null)
                throw new NullReferenceException(nameof(original));
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));
            
            if(original.IsComplete)
                throw new InvalidOperationException("callback will never call because routine is completed");
            if(original.IsError)
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
            
            if(original.IsComplete)
                throw new InvalidOperationException("callback will never call because routine is completed");
            if(original.IsError)
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
    }
}