using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Omega.Routines.Tests.Continuation
{
    public class CatchTests
    {
        [UnityTest]
        public IEnumerator NestedCancelledRoutineShouldStopParentRoutineByDefaultTest()
        {
            bool flag = false;

            IEnumerator CancelledRoutine(RoutineControl @this)
            {
                @this.GetRoutine().Cancel();
                yield break;
            }

            LogAssert.ignoreFailingMessages = true;

            yield return Routine.Sequence(
                Routine.ByAction(() => flag = true),
                Routine.ByEnumerator(CancelledRoutine),
                Routine.ByAction(Assert.Fail));

            LogAssert.ignoreFailingMessages = false;

            Assert.True(flag);
        }

        [UnityTest]
        public IEnumerator NestedRoutineWithErrorShouldStopParentRoutineByDefaultTest()
        {
            bool flag = false;

            IEnumerator RoutineWithError(RoutineControl @this)
            {
                throw new Exception("this is test exception");
                yield break;
            }

            LogAssert.ignoreFailingMessages = true;

            yield return Routine.Sequence(
                Routine.ByAction(() => flag = true),
                Routine.ByEnumerator(RoutineWithError),
                Routine.ByAction(Assert.Fail));

            LogAssert.ignoreFailingMessages = false;

            Assert.True(flag);
        }

        [UnityTest]
        public IEnumerator NestedCancelledRoutineWithCatchNotShouldStopParentRoutineTest()
        {
            bool flag = false;

            IEnumerator CancelledRoutine(RoutineControl @this)
            {
                @this.GetRoutine().Cancel();
                yield break;
            }

            LogAssert.ignoreFailingMessages = true;

            yield return Routine.Sequence(
                Routine.ByEnumerator(CancelledRoutine).Catch(CompletionCase.Canceled),
                Routine.ByAction(() => flag = true));

            LogAssert.ignoreFailingMessages = false;

            Assert.True(flag);
        }

        [UnityTest]
        public IEnumerator NestedRoutineWithErrorAndCatchNotShouldStopParentRoutineTest()
        {
            bool flag = false;

            IEnumerator ErrorRoutine(RoutineControl @this)
            {
                throw new Exception("its test exception");
                yield break;
            }

            LogAssert.ignoreFailingMessages = true;

            yield return Routine.Sequence(
                Routine.ByEnumerator(ErrorRoutine).Catch(CompletionCase.Error),
                Routine.ByAction(() => flag = true));

            LogAssert.ignoreFailingMessages = false;

            Assert.True(flag);
        }
    }
}