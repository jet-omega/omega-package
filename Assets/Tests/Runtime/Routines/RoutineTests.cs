using System;
using System.Collections;
using System.Text.RegularExpressions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Assert = NUnit.Framework.Assert;

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

        [Test]
        public void SelfCancellationWithForceCompletion2Test()
        {
            var flag = false;

            var routine = Routine.ByEnumerator(RoutineSteps);

            IEnumerator RoutineSteps(RoutineControl @this)
            {
                yield return Routine.ByAction(() => @this.GetRoutine().Cancel());
                Assert.Fail();
            }

            routine.Complete();
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
        public void ToStringShouldPrintRoutineStatusTest()
        {
            var routine = Routine.Empty();
            Assert.True(routine.IsNotStarted);
            Assert.True(routine.ToString().Contains("NotStarted"));
        }

        [Test]
        public void ToStringNotShouldPrintNameRoutineTest()
        {
            var routine = Routine.Empty();
            Assert.True(routine.IsNotStarted);
            Assert.False(routine.ToString().Contains("Name: "));
        }

        [Test]
        public void ToStringShouldPrintNameRoutineTest()
        {
            var routine = Routine.Empty();
            routine.SetName(nameof(ToStringShouldPrintNameRoutineTest));
            Assert.True(routine.IsNotStarted);
            Assert.True(routine.ToString().Contains("Name: " + nameof(ToStringShouldPrintNameRoutineTest)));
        }
        
        [Test]
        public void ToStringShouldPrintProgressTest()
        {
            IEnumerator Enumerator(RoutineControl @this)
            {
                @this.SetProgress(0.5f);
                yield return null;
            }

            var routine = Routine.ByEnumerator(Enumerator);
            RoutineUtilities.OneStep(routine);

            var progress = RoutineUtilities.GetProgressFromRoutine(routine);
            var progressString = progress.ToString("P");

            Assert.True(routine.ToString().Contains(progressString));
        }
    }
}