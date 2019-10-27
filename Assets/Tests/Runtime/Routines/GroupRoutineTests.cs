using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Omega.Routines.Tests
{
    public class GroupRoutineTests
    {
        [UnityTest]
        public IEnumerator RoutineShouldCompleteWhenHisRoutinesIsCompletedTest()
        {
            var routineWithDelay160ms = Routine.Delay(0.160f);
            var routineWithDelay150ms = Routine.Delay(0.150f);

            var startTestTime = Time.unscaledTime;

            yield return Routine.Group(routineWithDelay160ms, routineWithDelay150ms);

            var deltaTime = Time.unscaledTime - startTestTime;

            Assert.Greater(0.16f + 0.15f, deltaTime);
        }
    }
}