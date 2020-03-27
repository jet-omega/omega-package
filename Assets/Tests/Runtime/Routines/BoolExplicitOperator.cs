using System;
using System.Collections;
using System.Threading;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Omega.Routines.Tests
{
    public class BoolExplicitOperator
    {
        [UnityTest]
        public IEnumerator RoutineShouldBeTrueWhenRoutineIsNotProcessingTest()
        {
            yield return Routine.Delay(0.025f)
                .GetSelf(out var routine);

            Assert.True(routine);
        }

        [Test]
        public void RoutineShouldBeFalseWhenRoutineIsNotStartedTest()
        {
            var routine = Routine.Delay(0.025f);
            Assert.False(routine); // not started
        }

        [Test]
        public void RoutineShouldBeFalseWhenRoutineIsProcessingTest()
        {
            var routine = Routine.Task(() => Thread.Sleep(25)).StartTask();
            Assert.False(routine); // started, but not completed
        }

        [UnityTest]
        public IEnumerator RoutineShouldBeTrueWhenRoutineIsCompleted()
        {
            var routine = Routine.Delay(0.025f);
            yield return routine;
            Assert.True(routine); // completed
        }

        [UnityTest]
        public IEnumerator RoutineShouldBeTrueWhenRoutineIsError()
        {
            bool flag = false;
            var routine = Routine.Task(() => throw new Exception())
                .ExceptionHandler(_ => flag = true);
            yield return routine;
            Assert.True(flag);// invoked from exception handler
            Assert.True(routine); // error
        }
    }
}