using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Omega.Routines.Tests
{
    public class RoutineCallbackTests
    {
        [UnityTest]
        public IEnumerator CallbackShouldInvokedWhenRoutineIsComplete()
        {
            var flag = false;

            yield return Routine.Task(() => { }).Callback(() => flag = true);

            Assert.True(flag);
        }

        [UnityTest]
        public IEnumerator CallbackWithResultShouldInvokedWhenRoutineIsComplete()
        {
            var resultValue = 0;
            var taskValue = 1024;

            yield return Routine.Task(() =>
            {
                int i = 0;
                while (i != taskValue)
                    i++;

                return i;
            }).Callback((int i) => resultValue = i);

            Assert.AreEqual(taskValue, resultValue);
        }

        [Test]
        public void RoutineShouldProcessCallbackWhenEnumeratorIsNull()
        {
            var flag = false;
            var routine = new TestRoutine().Callback(() => flag = true);
            routine.Complete();
            Assert.True(flag);
        }

        private class TestRoutine : Routine
        {
            protected override IEnumerator RoutineUpdate() => null;
        }
    }
}