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