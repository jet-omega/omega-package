using System;
using System.Collections;
using JetBrains.Annotations;
using Omega.Routines;
using UnityEngine;

namespace Omega.Tools.Experimental.UtilitiesAggregator
{
    public sealed class RoutineUtilities
    {
        internal RoutineUtilities()
        {
        }

        /// <summary>
        /// Заставляет вызвавший поток ожидать выполнение рутины 
        /// </summary>
        /// <param name="routine">Рутина выполнение которой необходимо ожидать</param>
        /// <exception cref="NullReferenceException">routine указывает на null</exception>
        public void Complete(Routine routine)
        {
            if (!(routine is IEnumerator enumeratorRoutine))
                throw new NullReferenceException(nameof(routine));

            CompleteWithoutChecks(enumeratorRoutine);
        }

        public static void CompleteWithoutChecks([NotNull] IEnumerator routine)
        {
            while (routine.MoveNext())
                switch (routine.Current)
                {
                    case IEnumerator nestedRoutine:
                        CompleteWithoutChecks(nestedRoutine);
                        break;
                    case AsyncOperation asyncOperation:
                        WaitAsyncOperation(asyncOperation);
                        break;
                }
        }

        private static void WaitAsyncOperation([NotNull] AsyncOperation asyncOperation)
        {
            while (!asyncOperation.isDone);
        }
    }
}