using System;
using System.Collections;
using System.Threading;
using JetBrains.Annotations;
using Omega.Routines;
using UnityEngine;

namespace Omega.Routines
{
    internal sealed class RoutineUtilities
    {
        internal static void CompleteWithoutChecks(Routine routine)
        {
            routine.OnForcedCompleteInternal();
            CompleteNow(routine);
        }
        
        internal static void CompleteWithoutChecks(Routine routine, TimeSpan timeOut)
        {
            routine.OnForcedCompleteInternal();
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