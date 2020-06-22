using System;
using System.Collections;
using System.Text;
using System.Threading;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Omega.Routines.Tests
{
    public class SequenceRoutineTests
    {
        [UnityTest]
        public IEnumerator SequenceCompleteAfterEveryRoutineTest()
        {
            var routineWithDelay100Ms = Routine.Delay(TimeSpan.FromMilliseconds(100));
            var routineWithDelay150Ms = Routine.Delay(TimeSpan.FromMilliseconds(150));
            
            var startTestTime = DateTime.Now;

            yield return Routine.Sequence(routineWithDelay100Ms, routineWithDelay150Ms);

            var deltaTime = DateTime.Now - startTestTime;
            
            Assert.Greater(deltaTime, TimeSpan.FromMilliseconds(100 + 150));
        }

        [UnityTest]
        public IEnumerator SequenceRunsInOrderTest()
        {
            long firstRoutineEndedTicks = 0, secondRoutineEndedTicks = 0;
            
            var firstRoutine = Routine.Delay(TimeSpan.FromMilliseconds(10)).Callback(() => firstRoutineEndedTicks = DateTime.Now.Ticks);
            var secondRoutine = Routine.Delay(TimeSpan.FromMilliseconds(10)).Callback(() => secondRoutineEndedTicks = DateTime.Now.Ticks);

            yield return Routine.Sequence(firstRoutine, secondRoutine);
            
            Assert.Greater(secondRoutineEndedTicks, firstRoutineEndedTicks);
        }

        [UnityTest]
        public IEnumerator EmptyRoutinesParamsArrayCompletes()
        {
            yield return Routine.Sequence(Array.Empty<Routine>()).Self(out var sequenceRoutine);
            
            Assert.IsTrue(sequenceRoutine.IsComplete);
            Assert.IsFalse(sequenceRoutine.IsError);
        }
        
        [UnityTest]
        public IEnumerator CompleteAndIncompleteRoutinesCompletes()
        {
            var incompleteRoutine = Routine.Delay(TimeSpan.FromMilliseconds(10));
            var completeRoutine = Routine.FromCompleted();

            yield return Routine.Sequence(incompleteRoutine, completeRoutine).Self(out var sequenceRoutine);
            
            Assert.True(sequenceRoutine.IsComplete);
            Assert.IsFalse(sequenceRoutine.IsError);
        }
    }
}