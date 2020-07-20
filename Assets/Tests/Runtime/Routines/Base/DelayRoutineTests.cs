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
            var delayTime = TimeSpan.FromMilliseconds(50);

            var startTime = DateTime.UtcNow;

            yield return Routine.Delay(delayTime);

            var passedTime = (DateTime.UtcNow - startTime).TotalSeconds;

            Assert.GreaterOrEqual(passedTime, delayTime.TotalSeconds);
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
            var delayTime = TimeSpan.FromMilliseconds(50);

            var startTime = DateTime.UtcNow;

            Routine.Delay(delayTime).Complete(5);

            var passedTime = (DateTime.UtcNow - startTime).TotalSeconds;

            Assert.GreaterOrEqual(passedTime, delayTime.TotalSeconds);
        }

        [UnityTest]
        public IEnumerator CancelDelayRoutineShouldBeCancelTest()
        {
            var timeMeter = TimeMeter.New();
            var routine = Routine.Delay(1);
            routine.SetName("delay").Cancel();
            yield return routine;

            var elapsed = timeMeter.ToSeconds();
            Assert.Less(elapsed, 1f);
        }
        
        [Test]
        public void ChildDelayRoutineHasProgressOnFirstParentGetProgress()
        {
            var routine = Routine.Delay(0.05f);
            Routine.WaitOne(routine, () => true)
                .OnProgress(p =>
                {
                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    if (p == 0)
                        Assert.Pass();
                    else
                        Assert.Fail();
                });
        }

        [Test]
        public void DelayRoutineHasProgressBeforeStart()
        {
            var routine = Routine.Delay(0.05f);
            var p = routine.GetProgress();
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            Assert.IsTrue(p == 0);
        }
    }
}