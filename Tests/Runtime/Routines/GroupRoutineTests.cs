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
            var routineWithDelay160ms = Routine.Task(() => Thread.Sleep(160));
            var routineWithDelay150ms = Routine.Task(() => Thread.Sleep(150));

            var startTestTime = DateTime.Now;

            yield return Routine.Group(routineWithDelay160ms, routineWithDelay150ms);

            var deltaTime = DateTime.Now - startTestTime;

            Debug.Log("Delta: " + deltaTime.TotalMilliseconds);
            
            Assert.Greater(0.16 + 0.15, deltaTime.TotalSeconds);
        }
    }
}