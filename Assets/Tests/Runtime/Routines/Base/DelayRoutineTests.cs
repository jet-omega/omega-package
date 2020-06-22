using System;
using System.Collections;
using NUnit.Framework;
using Omega.Package;
using UnityEngine;
using UnityEngine.TestTools;

namespace Omega.Routines.Tests
{
    public class DelayRoutineTests
    {
        [UnityTest]
        public IEnumerator DelayRoutineShouldFulfilledSetTimeTest()
        {
            var targetTime = TimeSpan.FromMilliseconds(50);

            var startTime = DateTime.Now;
            var scheduledReleaseTime = startTime + targetTime;

            yield return Routine.Delay(targetTime);

            var delta = (DateTime.Now - scheduledReleaseTime).TotalSeconds;
            Assert.Positive(delta);
            Assert.GreaterOrEqual(Time.unscaledDeltaTime, delta);
        }

        [Test]
        public void DelayRoutineShouldCompleteWhenUserUseForceCompleteTest()
        {
            var targetTime = TimeSpan.FromMilliseconds(50);
            Routine.Delay(targetTime).Complete();
        }

        [Test]
        public void DelayRoutineShouldFulfilledSetTimeWhenUserUseForceCompleteTest()
        {
            var targetTime = TimeSpan.FromMilliseconds(50);

            var startTime = DateTime.Now;
            var scheduledReleaseTime = startTime + targetTime;

            Routine.Delay(targetTime).Complete();

            var delta = (DateTime.Now - scheduledReleaseTime).TotalSeconds;
            Assert.Positive(delta);
            Assert.GreaterOrEqual(Time.unscaledDeltaTime, delta);
        }

        [UnityTest]
        public IEnumerator CancelDelayRoutineShouldBeCancelTest()
        {
            var timeMeter = TimeMeter.New();
            var routine = Routine.Delay(1);
            routine.Cancel();
            yield return routine;

            var elapsed = timeMeter.ToSeconds();
            Assert.Less(elapsed, 1f);
        }
    }
}