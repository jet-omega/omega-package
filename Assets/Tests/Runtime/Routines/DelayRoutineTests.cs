using System;
using System.Collections;
using NUnit.Framework;
using Omega.Experimental;
using UnityEngine;
using UnityEngine.TestTools;

namespace Omega.Routines.Tests
{
    public class DelayRoutineTests
    {
        [UnityTest]
        public IEnumerator DelayRoutineShouldFulfilledSetTimeTest()
        {
            var targetTime = Utilities.Time.FromMilliseconds(50);

            var startTime = DateTime.Now;
            var scheduledReleaseTime = startTime + TimeSpan.FromSeconds(targetTime);

            yield return Routine.Delay(targetTime);

            var delta = (DateTime.Now - scheduledReleaseTime).TotalSeconds;
            Assert.Positive(delta);
            Assert.GreaterOrEqual(Time.unscaledDeltaTime, delta);
        }

        [Test]
        public void DelayRoutineShouldCompleteWhenUserUseForceCompleteTest()
        {
            var targetTime = Utilities.Time.FromMilliseconds(50);
            Routine.Delay(targetTime).Complete();
        }
        
        [Test]
        public void DelayRoutineShouldFulfilledSetTimeWhenUserUseForceCompleteTest()
        {
            var targetTime = Utilities.Time.FromMilliseconds(50);

            var startTime = DateTime.Now;
            var scheduledReleaseTime = startTime + TimeSpan.FromSeconds(targetTime);

            Routine.Delay(targetTime).Complete();

            var delta = (DateTime.Now - scheduledReleaseTime).TotalSeconds;
            Assert.Positive(delta);
            Assert.GreaterOrEqual(Time.unscaledDeltaTime, delta);
        }
    }
}