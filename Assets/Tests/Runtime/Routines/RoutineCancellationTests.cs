using System;
using System.Collections;
using System.Text.RegularExpressions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Omega.Routines.Tests
{
    public class RoutineCancellationTests
    {
        [Test]
        public void RoutineShouldThrowExceptionIfNestedRoutineIsCanceledTest()
        {
            var nestedRoutine = Routine.Delay(10);
            nestedRoutine.Cancel();

            IEnumerator TestRoutine(Routine nested)
            {
                yield return nested;
            }

            var routine = Routine.ByEnumerator(TestRoutine(nestedRoutine));

            LogAssert.Expect(LogType.Error, new Regex("."));
            routine.Complete(1);
        }
    }
}