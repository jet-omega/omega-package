using System.Collections;
using NUnit.Framework;
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
            float delay50MsRoutineProgress = 0, anyRoutineProgress = 0;
            var delay50MsRoutine = Routine.Delay(0.05f)
                .OnProgress(p => delay50MsRoutineProgress = p);
            var delay100MsRoutine = Routine.Delay(0.1f);

            yield return Routine.Any(delay50MsRoutine, delay100MsRoutine)
                .OnProgress(p =>
                {
                    anyRoutineProgress = p;
                    Assert.AreEqual(delay50MsRoutineProgress, anyRoutineProgress);
                });
        }
    }
}