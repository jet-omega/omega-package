using System;
using System.Collections;
using NUnit.Framework;
using Omega.Experimental;
using Omega.Package.Internal;

namespace Omega.Routines.Tests
{
    public class SynchronousWaitingRoutineTests
    {
        [Test]
        public void DelayRoutineShouldCompleteSynchronousTest()
        {
            var delay = 0.15f;

            var startTime = DateTime.Now;
            Routine.Delay(delay).Complete();
            var deltaTime = (DateTime.Now - startTime).TotalSeconds;

            Assert.Greater(deltaTime, delay);
        }

        [Test]
        public void ComplexRoutineShouldCompleteSynchronousTest()
        {
            var complexLevel = 5;

            Routine CreateRoutine(int level, float delay)
            {
                return level == 0
                    ? (Routine) Routine.Delay(delay)
                    : new TestRoutine(CreateRoutine(level - 1, delay));
            }

            var startTime = DateTime.Now;
            CreateRoutine(complexLevel, 0.15f).Complete();
            var deltaTime = (DateTime.Now - startTime).TotalSeconds;

            Assert.Greater(deltaTime, 0.15f);
        }

        [Test]
        public void SynchronousCompleteRoutineShouldThrowExceptionByTimeoutTest()
        {
            var timeOut = 0.175f;
            var delay = timeOut * 2 + 1;
            var routine = Routine.Delay(delay);
            Assert.Throws<TimeoutException>(() => routine.Complete(timeOut));
        }

        [Test]
        public void CompleteShouldInvokeInternalCallbackTest()
        {
            var routine = new TestCallRoutine();
            routine.Complete();
            Assert.True(routine.WereForced);
        }
        
        [Test]
        public void CompleteShouldInvokeNestedInternalCallbackTest()
        {
            var nestedRoutine = new TestCallRoutine();
            var rootRoutine = new TestRoutine(nestedRoutine);

            rootRoutine.Complete();
            
            Assert.True(nestedRoutine.IsComplete);
            Assert.True(nestedRoutine.WereForced);
        }

        private sealed class TestRoutine : Routine
        {
            private Routine targetRoutine;

            public TestRoutine(Routine targetRoutine)
            {
                this.targetRoutine = targetRoutine;
            }

            protected override IEnumerator RoutineUpdate()
            {
                yield return targetRoutine;
            }
        }
        
        private sealed class TestCallRoutine : Routine
        {
            public bool WereForced { private set; get; }

            protected override IEnumerator RoutineUpdate()
            {
                return null;
            }

            protected override void OnForcedComplete()
            {
                WereForced = true;
            }
        }
    }
}