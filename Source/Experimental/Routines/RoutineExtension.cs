using System;

namespace Omega.Experimental.Routines
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

        public static TRoutine ExceptionHandler<TRoutine>(this TRoutine original, Action<Exception> exceptionHandler)
            where TRoutine : Routine
        {
            if (original == null)
                throw new NullReferenceException(nameof(original));
            if (original == null)
                throw new ArgumentNullException(nameof(exceptionHandler));

            original.SetExceptionHandlerInternal(exceptionHandler);

            return original;
        }
    }
}