using System;
using System.Collections;
using System.Text.RegularExpressions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Omega.Routines.Tests
{
    public class TimeoutRoutineTests
    {
        [Test]
        public void TimeoutRoutineThrowsExceptionOnTimeout()
        {
            var internalRoutine = Routine.Delay(0.5f);
            var timeoutRoutine = Routine.Timeout(internalRoutine, 0.2f);
            LogAssert.Expect(LogType.Error, new Regex(nameof(TimeoutException)));
            timeoutRoutine.Complete();
            Assert.True(timeoutRoutine.IsError);
        }

        [Test]
        public void TimeoutRoutineCompletesIfNoTimeout()
        {
            var internalRoutine = Routine.Delay(0.2f);
            var timeoutRoutine = Routine.Timeout(internalRoutine, 0.5f);
            timeoutRoutine.Complete();
            Assert.IsTrue(timeoutRoutine.IsComplete);
        }

        [UnityTest]
        public IEnumerator TimeoutRoutineWithResultThrowsExceptionOnTimeout()
        {
            IEnumerator Enumerator(RoutineControl<int> @this)
            {
                yield return Routine.Delay(0.1f);
                @this.SetResult(1);
                yield return Routine.Delay(0.1f);
                @this.SetResult(2);
                yield return Routine.Delay(0.1f);
                @this.SetResult(3);
                yield return Routine.Delay(0.1f);
                @this.SetResult(4);
                yield return Routine.Delay(0.1f);
                @this.SetResult(5);
            }

            var internalRoutine = Routine.ByEnumerator<int>(Enumerator);
            var timeoutRoutine = Routine.Timeout(internalRoutine, 0.2f);
            LogAssert.Expect(LogType.Error, new Regex(nameof(TimeoutException)));
            yield return timeoutRoutine;
            Assert.True(timeoutRoutine.IsError);
        }
    }
}