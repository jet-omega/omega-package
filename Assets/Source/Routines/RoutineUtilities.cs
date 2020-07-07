using System;
using System.Collections;
using JetBrains.Annotations;

namespace Omega.Routines
{
    internal sealed class RoutineUtilities
    {
        internal static void CompleteWithoutChecks(Routine routine)
        {
            routine.SetupForcedProcessingInternal();
            CompleteNow(routine);
        }
        
        internal static void CompleteWithoutChecks(Routine routine, TimeSpan timeOut)
        {
            routine.SetupForcedProcessingInternal();
            CompleteNow(routine, timeOut);
        }

        internal static bool OneStep(IEnumerator routine)
        {
            return routine.MoveNext();
        }
        
        
        private static void CompleteNow([NotNull] IEnumerator routine)
        {
            while (routine.MoveNext()) ;
        }

        private static void CompleteNow([NotNull] IEnumerator routine, TimeSpan timeOut)
        {
            var startSynchronousWaitingTime = DateTime.UtcNow;
            while (routine.MoveNext())
            {
                var currentTime = DateTime.UtcNow;
                var deltaTime = currentTime - startSynchronousWaitingTime;
                if (deltaTime >= timeOut)
                    throw new TimeoutException("Synchronous wait routine was canceled due to timeout");
            }
        }

        internal static float GetProgressFromRoutine(Routine routine)
        {
            return routine is IProgressRoutineProvider progressRoutineProvider
                ? progressRoutineProvider.GetProgress()
                : routine.IsComplete ? 1 : 0;
        }
        
        internal static IProgressRoutineProvider GetProgressRoutineProvider(Routine routine)
        {
            return routine is IProgressRoutineProvider progressRoutineProvider
                ? progressRoutineProvider
                : new ProgressProviderAdapter(routine);
        }
    }
}