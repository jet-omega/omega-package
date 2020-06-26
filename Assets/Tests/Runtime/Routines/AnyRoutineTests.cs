using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Omega.Routines.Tests
{
    public class AnyRoutineTests
    {
        [UnityTest]
        public IEnumerator AnyRoutineCompletesOnAnyRoutineComplete()
        {
            var delay50MsRoutine = Routine.Delay(0.05f);
            var delay100MsRoutine = Routine.Delay(0.1f);

            yield return Routine.Any(delay50MsRoutine, delay100MsRoutine);

            Assert.IsTrue(delay50MsRoutine.IsComplete && !delay100MsRoutine.IsComplete);
        }

        [UnityTest]
        public IEnumerator AnyRoutineProgressEqualsMaxRoutineProgress()
        {
            float delay50MsRoutineProgress, delay100MsRoutineProgress, anyRoutineProgress;
            var delay50MsRoutine = Routine.Delay(0.05f);
            var delay100MsRoutine = Routine.Delay(0.1f);

            yield return Routine.Any(delay50MsRoutine, delay100MsRoutine)
                .OnProgress(p =>
                {
                    anyRoutineProgress = p;
                    delay50MsRoutineProgress = delay50MsRoutine.GetProgress();
                    delay100MsRoutineProgress = delay100MsRoutine.GetProgress();
                    Assert.AreEqual(
                        Mathf.Max(delay50MsRoutineProgress, delay100MsRoutineProgress),
                        anyRoutineProgress);
                });
        }
    }
}