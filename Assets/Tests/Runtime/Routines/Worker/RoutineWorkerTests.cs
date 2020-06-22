using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Omega.Routines.Tests
{
    public class RoutineWorkerTests
    {
        [UnityTest]
        public IEnumerator DelayInBackgroundShouldCompleteTest()
        {
            var interval = TimeSpan.FromMilliseconds(100);
            var delay = Routine.Delay(interval);
            delay.InBackground();

            var startTime = Time.time;
            while (!delay.IsComplete && startTime + interval.TotalSeconds * 2 > Time.time)
                yield return null;
            
            Assert.True(delay.IsComplete);
        }

        [UnityTest]
        public IEnumerator ActionInBackgroundShouldCompleteTest()
        {
            var routine = Routine.ByAction(() =>
            {
                int i = 1 + 1;
                return i;
            });

            routine.InBackground();
            
            yield return null;

            Assert.True(routine.IsComplete);
        }
     }
}