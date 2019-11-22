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
        internal static void CompleteWithoutChecks([NotNull] IEnumerator routine)
        {
            while (routine.MoveNext()) ;
        }

        internal static void CompleteWithoutChecks([NotNull] IEnumerator routine, TimeSpan timeOut)
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
    }
}