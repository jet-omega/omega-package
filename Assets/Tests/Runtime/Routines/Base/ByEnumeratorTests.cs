using System;
using System.Collections;
using NUnit.Framework;
using Omega.Package;
using UnityEngine.TestTools;

namespace Omega.Routines.Tests
{
    public class ByEnumeratorTests
    {
        private bool flagByEnumeratorShouldProcessCoroutineTest;

        private bool flagByEnumeratorWithControlShouldProvideRoutineTest;
//        private bool flagByEnumeratorWithArgAndRoutineControlShouldProvideRoutine;

        [UnityTest]
        public IEnumerator ByEnumeratorShouldProcessCoroutineTest()
        {
            flagByEnumeratorShouldProcessCoroutineTest = false;

            yield return Routine.ByEnumerator(ByEnumeratorShouldProcessCoroutine());

            Assert.True(flagByEnumeratorShouldProcessCoroutineTest);

            flagByEnumeratorShouldProcessCoroutineTest = false;
        }

        [UnityTest]
        public IEnumerator ByEnumeratorWithControlShouldProvideRoutineTest()
        {
            flagByEnumeratorWithControlShouldProvideRoutineTest = false;

            yield return Routine.ByEnumerator(ByEnumeratorWithControlShouldProvideRoutine);

            Assert.True(flagByEnumeratorWithControlShouldProvideRoutineTest);

            flagByEnumeratorWithControlShouldProvideRoutineTest = false;
        }

        [UnityTest]
        public IEnumerator ByEnumeratorWithResultShouldProvideResultTest()
        {
            yield return Routine.ByEnumerator<bool>(ByEnumeratorWithResultShouldProvideResult)
                .Result(out var result);

            Assert.True(result.Result);
        }

        [Test]
        public void RoutineControlShouldThrowExceptionWhenCallSetResultAndRoutineIsNotDefinedTest()
        {
            var routineControl = new RoutineControl<bool>();
            Assert.Throws(ExceptionHelper.SetResultCannotCalledWhenRoutineIsNotDefined.GetType(),
                () => routineControl.SetResult(true),
                ExceptionHelper.Messages.SetResultCannotCalledWhenRoutineIsNotDefined,
                Array.Empty<object>());
        }

        [UnityTest]
        public IEnumerator ByEnumeratorWithArgAndRoutineControlShouldProvideRoutineTest()
        {
//            flagByEnumeratorWithArgAndRoutineControlShouldProvideRoutine = false;
            yield return Routine.ByEnumerator(ByEnumeratorWithArgAndRoutineControlShouldProvideRoutine, true);
//            flagByEnumeratorWithArgAndRoutineControlShouldProvideRoutine = false;
        }


        [UnityTest]
        public IEnumerator ByEnumeratorWithArgAndRoutineControlResultShouldProvideRoutineTest()
        {
            yield return Routine.ByEnumerator<bool, bool>(
                    ByEnumeratorWithArgAndRoutineControlResultShouldProvideRoutine, true)
                .Result(out var result);

            Assert.True(result.Result);
        }

        [Test]
        public void RoutineControlShouldSetupProgressTest()
        {
            const int steps = 10;

            IEnumerator Enumerator(RoutineControl @this)
            {
                int i = 0;
                while (i < steps)
                {
                    @this.SetProgress(i++ / (float) steps);
                    yield return null;
                }
            }

            var routine = Routine.ByEnumerator(Enumerator);

            for (int i = 0; i < steps / 2; i++)
                RoutineUtilities.OneStep(routine);

            const float expectedProgress = (steps / 2 - 1) / (float) steps;

            Assert.AreEqual(expectedProgress, routine.GetProgress());
        }
        
        [Test]
        public void RoutineControlShouldProvideCancellationEventTest()
        {
            bool wasCanceled = false;
            
            IEnumerator Enumerator(RoutineControl @this)
            {
                @this.OnCancellationCallback(() => { wasCanceled = true; });
                yield return null;
                yield return null;
            }

            var routine = Routine.ByEnumerator(Enumerator);
            RoutineUtilities.OneStep(routine);
            
            routine.Cancel();
            
            Assert.True(routine.IsCanceled);
            Assert.True(wasCanceled);
        }

        private IEnumerator ByEnumeratorWithArgAndRoutineControlShouldProvideRoutine(bool arg,
            RoutineControl routineControl)
        {
            Assert.True(arg);
            Assert.NotNull(routineControl.GetRoutine());
            yield return null;
        }

        private IEnumerator ByEnumeratorWithArgAndRoutineControlResultShouldProvideRoutine(bool arg,
            RoutineControl<bool> routineControl)
        {
            Assert.True(arg);
            routineControl.SetResult(true);
            yield return null;
        }

        private IEnumerator ByEnumeratorWithResultShouldProvideResult(RoutineControl<bool> routineControl)
        {
            yield return null;
            yield return null;
            routineControl.SetResult(true);
        }

        private IEnumerator ByEnumeratorShouldProcessCoroutine()
        {
            yield return null;
            yield return null;
            flagByEnumeratorShouldProcessCoroutineTest = true;
        }

        private IEnumerator ByEnumeratorWithControlShouldProvideRoutine(RoutineControl routineControl)
        {
            Assert.NotNull(routineControl.GetRoutine());
            yield return null;
            yield return null;
            flagByEnumeratorWithControlShouldProvideRoutineTest = true;
        }
    }
}