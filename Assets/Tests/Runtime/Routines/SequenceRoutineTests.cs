using System;
using System.Collections;
using NUnit.Framework;
using Omega.Package;
using UnityEngine.TestTools;

namespace Omega.Routines.Tests
{
    public class SequenceRoutineTests
    {
        [UnityTest]
        public IEnumerator SequenceCompleteAfterEveryRoutineTest()
        {
            var routineWithDelay100Ms = Routine.Delay(0.1f);
            var routineWithDelay150Ms = Routine.Delay(0.15f);
            
            var timeMeter = TimeMeter.New();
            timeMeter.Start();
            yield return Routine.Sequence(routineWithDelay100Ms, routineWithDelay150Ms);
            var deltaTime = timeMeter.ToMilliseconds();
            
            Assert.Greater(deltaTime, 0.1f + 0.15f);
        }

        [UnityTest]
        public IEnumerator SequenceRunsInOrderTest()
        {
            long firstRoutineEndedTicks = 0, secondRoutineStartedTicks = 0;
            
            var firstRoutine = Routine.Delay(0.1f).Callback(() => firstRoutineEndedTicks = DateTime.Now.Ticks);
            var secondRoutine = Routine.Action(() => secondRoutineStartedTicks = DateTime.Now.Ticks);

            yield return Routine.Sequence(firstRoutine, secondRoutine);
            
            Assert.GreaterOrEqual(secondRoutineStartedTicks, firstRoutineEndedTicks);
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
            var incompleteRoutine = Routine.Delay(0.01f);
            var completeRoutine = Routine.FromCompleted();

            yield return Routine.Sequence(incompleteRoutine, completeRoutine).Self(out var sequenceRoutine);
            
            Assert.True(sequenceRoutine.IsComplete);
            Assert.IsFalse(sequenceRoutine.IsError);
        }
    }
}
