using System;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Omega.Routines.Tests
{
    public class RoutineTests
    {
        [Test]
        public void RoutineShouldThrowExceptionIfNestedRoutineHaveErrorTest()
        {
            var routineWithError = Routine.ByAction(() => throw new Exception("Its test exception"));
            LogAssert.Expect(LogType.Error, new Regex("."));
            routineWithError.Complete();


            IEnumerator TestRoutine(RoutineControl<int> control)
            {
                yield return routineWithError;
                control.SetResult(50);
            }

            var routine = Routine.ByEnumerator<int>(TestRoutine);
            LogAssert.Expect(LogType.Error, new Regex("."));
            routine.Complete();
            Assert.True(routine.IsError);
        }

        [UnityTest]
        public IEnumerator NumberRoutineOfUpdatesShouldNotDependOnMoveNext()
        {
            int i = 0;

            var routine = Routine.FromResult(0);
            routine.AddUpdateActionInternal(() => i++);
            yield return routine;

            var oneMoveNextUpdateCount = i;

            i = 0;
            routine = Routine.FromResult(0);
            routine.AddUpdateActionInternal(() => i++);
            yield return routine;
            yield return routine;
            yield return routine;
            yield return routine;
            yield return routine;

            var fiveMoveNextUpdateCount = i;

            Assert.AreEqual(oneMoveNextUpdateCount, fiveMoveNextUpdateCount);
        }

        [UnityTest]
        public IEnumerator CallbackShouldBeCalledOnceTest()
        {
            int i = 0;

            var routine = Routine.FromResult(0);
            routine.Callback(() => i++);

            yield return routine;
            yield return routine;
            yield return routine;
            yield return routine;
            yield return routine;

            Assert.AreEqual(1, i);
        }
    }
}