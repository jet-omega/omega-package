using System;
using System.Collections;
using System.Text.RegularExpressions;
using NUnit.Framework;
using UnityEditor.VersionControl;
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
                yield return routineWithError.Catch(CompletionCase.Error);
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


            LogAssert.ignoreFailingMessages = true;
            Routine.WaitOne(routine, () => 1).Complete();
            LogAssert.ignoreFailingMessages = false;
            
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

        [Test]
        public void ExecuteCompletedRoutineNotShouldFailTest()
        {
            var completed = Routine.FromCompleted();
            Assert.True(completed.IsComplete);

            RoutineUtilities.OneStep(completed);
        }

        [UnityTest]
        public IEnumerator NestedEnumeratorTest()
        {
            bool flag = false;
            
            IEnumerator RoutineSteps()
            {
                IEnumerator NestedRoutineSteps()
                {
                    yield return null;
                    flag = true;
                    yield return null;
                }

                yield return NestedRoutineSteps();
            }

            yield return Routine.ByEnumerator(RoutineSteps());
            
            Assert.True(flag);
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

            yield return Routine.WaitOne(routine.Catch(CompletionCase.Canceled), () => 1);

            Assert.False(flag);
        }

        [UnityTest]
        public IEnumerator NestedRoutineShouldBeIsProcessingTest()
        {
            var nestedRoutine = Routine.Delay(0.1f);
            
            IEnumerator RoutineSteps()
            {
                yield return nestedRoutine;
                yield return null;
            }

            Routine.ByEnumerator(RoutineSteps()).InBackground();
            yield return null;
            yield return null;
            Assert.False(nestedRoutine.IsComplete);
            Assert.True(nestedRoutine.IsProcessing);

        }

        [UnityTest]
        public IEnumerator RoutineShouldBeIsProcessing()
        {
            var nestedRoutine = Routine.Delay(0.1f);
            nestedRoutine.InBackground();
            
            yield return null;
            yield return null;
            
            Assert.False(nestedRoutine.IsComplete);
            Assert.True(nestedRoutine.IsProcessing);
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