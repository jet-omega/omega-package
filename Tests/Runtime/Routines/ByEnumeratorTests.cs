using System;
using System.Collections;
using NUnit.Framework;
using Omega.Experimental.Event;
using UnityEngine.TestTools;

namespace Omega.Routines.Tests
{
    public class ByEnumeratorTests
    {
        private bool flagByEnumeratorShouldProcessCoroutineTest;
        private bool flagByEnumeratorWithControlShouldProvideRoutineTest;

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