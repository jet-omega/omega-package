using System;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.TestTools;

namespace Omega.Routines.Tests
{
    public class RoutineTests
    {
        [Test]
        public void RoutineShouldThrowExceptionIfNestedRoutineHaveErrorTest()
        {
            var routineWithError = Routine.ByAction(() => throw new Exception("It's a test exception"));
            LogAssert.Expect(LogType.Error, new Regex("."));
            routineWithError.Complete();
            Assert.True(routineWithError.IsError);
            
            IEnumerator TestRoutine(RoutineControl<int> control)
            {
                yield return routineWithError;
                control.SetResult(50);
            }

            var routine = Routine.ByEnumerator<int>(TestRoutine);
            routine.Complete();
            Assert.False(routine.IsError);
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

        [Test]
        public void SelfCancellationWithForceCompletionTest()
        {
            var flag = false;

            var routine = Routine.ByEnumerator(RoutineSteps);

            IEnumerator RoutineSteps(RoutineControl @this)
            {
                @this.GetRoutine().Cancel();
                yield return Routine.ByAction(() => flag = true);
            }

            Routine.WaitOne(routine, () => 1).Complete();
            Assert.False(flag);
        }

        [UnityTest]
        public IEnumerator SelfCancellationTest()
        {
            var flag = false;

            var routine = Routine.ByEnumerator(RoutineSteps);

            IEnumerator RoutineSteps(RoutineControl @this)
            {
                @this.GetRoutine().Cancel();
                yield return Routine.ByAction(() => flag = true);
            }

            yield return Routine.WaitOne(routine, () => 1);

            Assert.False(flag);
        }

        [Test]
        public void SetNameTest()
        {
            var routine = Routine.Empty();
            routine.Name = "test routine name";
            var toString = routine.ToString();
            Debug.Log(toString);
            Assert.IsTrue(toString.Contains("test routine name"));
        }
    }
}