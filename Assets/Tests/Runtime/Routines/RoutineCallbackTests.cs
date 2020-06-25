using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

// ReSharper disable RedundantArgumentDefaultValue

namespace Omega.Routines.Tests
{
    public class RoutineCallbackTests
    {
        [UnityTest]
        public IEnumerator CallbackShouldInvokedWhenRoutineIsComplete()
        {
            var flag = false;

            yield return Routine.Task(() => { }).Callback(() => flag = true, CallbackCase.Complete);

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
            }).Callback(i => resultValue = i, CallbackCase.Complete);

            Assert.AreEqual(taskValue, resultValue);
        }

        [Test]
        public void RoutineShouldProcessCallbackWhenEnumeratorIsNull()
        {
            var flag = false;
            var routine = new TestRoutine().Callback(() => flag = true, CallbackCase.Complete);
            routine.Complete();
            Assert.True(flag);
        }

        [Test]
        public void CompletedRoutineShouldInvokeCallbackIfCallbackCaseIsComplete()
        {
            var result = false;
            void CallbackAction() => result = true;

            void RoutineAction()
            {
            }

            Routine.ByAction(RoutineAction).Callback(CallbackAction, CallbackCase.Complete).Complete();
            Assert.IsTrue(result);
        }

        [Test]
        public void CompletedRoutineShouldNotInvokeCallbackIfCallbackCaseIsError()
        {
            var result = false;
            void CallbackAction() => result = true;

            void RoutineAction()
            {
            }

            Routine.ByAction(RoutineAction).Callback(CallbackAction, CallbackCase.Error).Complete();
            Assert.IsFalse(result);
        }

        [Test]
        public void CompletedRoutineShouldNotInvokeCallbackIfCallbackCaseIsCancel()
        {
            var result = false;
            void CallbackAction() => result = true;

            void RoutineAction()
            {
            }

            Routine.ByAction(RoutineAction).Callback(CallbackAction, CallbackCase.Cancel).Complete();
            Assert.IsFalse(result);
        }

        [Test]
        public void CompletedRoutineShouldInvokeCallbackIfCallbackCaseIsNotComplete()
        {
            var result = false;
            void CallbackAction() => result = true;

            void RoutineAction()
            {
            }

            Routine.ByAction(RoutineAction).Callback(CallbackAction, CallbackCase.NotComplete).Complete();
            Assert.IsFalse(result);
        }

        [Test]
        public void CompletedRoutineShouldInvokeCallbackIfCallbackCaseIsAny()
        {
            var result = false;
            void CallbackAction() => result = true;

            void RoutineAction()
            {
            }

            Routine.ByAction(RoutineAction).Callback(CallbackAction, CallbackCase.Any).Complete();
            Assert.IsTrue(result);
        }

        [Test]
        public void RoutineWithErrorShouldNotInvokeCallbackIfCallbackCaseIsComplete()
        {
            LogAssert.ignoreFailingMessages = true;
            var result = false;
            void CallbackAction() => result = true;
            void RoutineAction() => throw new Exception();

            Routine.ByAction(RoutineAction).Callback(CallbackAction, CallbackCase.Complete).Complete();
            Assert.IsFalse(result);
        }

        [Test]
        public void RoutineWithErrorShouldInvokeCallbackIfCallbackCaseIsError()
        {
            LogAssert.ignoreFailingMessages = true;
            var result = false;
            void CallbackAction() => result = true;
            void RoutineAction() => throw new Exception();

            Routine.ByAction(RoutineAction).Callback(CallbackAction, CallbackCase.Error).Complete();
            Assert.IsTrue(result);
        }

        [Test]
        public void RoutineWithErrorShouldNotInvokeCallbackIfCallbackCaseIsCancel()
        {
            LogAssert.ignoreFailingMessages = true;
            var result = false;
            void CallbackAction() => result = true;
            void RoutineAction() => throw new Exception();

            Routine.ByAction(RoutineAction).Callback(CallbackAction, CallbackCase.Cancel).Complete();
            Assert.IsFalse(result);
        }

        [Test]
        public void RoutineWithErrorShouldInvokeCallbackIfCallbackCaseIsNotComplete()
        {
            LogAssert.ignoreFailingMessages = true;
            var result = false;
            void CallbackAction() => result = true;
            void RoutineAction() => throw new Exception();

            Routine.ByAction(RoutineAction).Callback(CallbackAction, CallbackCase.NotComplete).Complete();
            Assert.IsTrue(result);
        }

        [Test]
        public void RoutineWithErrorShouldInvokeCallbackIfCallbackCaseIsAny()
        {
            LogAssert.ignoreFailingMessages = true;
            var result = false;
            void CallbackAction() => result = true;
            void RoutineAction() => throw new Exception();

            Routine.ByAction(RoutineAction).Callback(CallbackAction, CallbackCase.Any).Complete();
            Assert.IsTrue(result);
        }

        [Test]
        public void CancelledRoutineShouldNotInvokeCallbackIfCallbackCaseIsComplete()
        {
            LogAssert.ignoreFailingMessages = true;
            var result = false;
            void CallbackAction() => result = true;

            IEnumerator Enumerator(RoutineControl routineControl)
            {
                yield return null;
                routineControl.GetRoutine().Cancel();
            }

            Assert.Throws<InvalidOperationException>(() =>
                Routine.ByEnumerator(Enumerator).Callback(CallbackAction, CallbackCase.Complete).Complete());
            Assert.IsFalse(result);
        }

        [Test]
        public void CancelledRoutineShouldNotInvokeCallbackIfCallbackCaseIsError()
        {
            LogAssert.ignoreFailingMessages = true;
            var result = false;
            void CallbackAction() => result = true;

            IEnumerator Enumerator(RoutineControl routineControl)
            {
                yield return null;
                routineControl.GetRoutine().Cancel();
            }

            Assert.Throws<InvalidOperationException>(() =>
                Routine.ByEnumerator(Enumerator).Callback(CallbackAction, CallbackCase.Error).Complete());
            Assert.IsFalse(result);
        }

        [Test]
        public void CancelledRoutineShouldInvokeCallbackIfCallbackCaseIsCancel()
        {
            LogAssert.ignoreFailingMessages = true;
            var result = false;
            void CallbackAction() => result = true;

            IEnumerator Enumerator(RoutineControl routineControl)
            {
                yield return null;
                routineControl.GetRoutine().Cancel();
            }

            Assert.Throws<InvalidOperationException>(() =>
                Routine.ByEnumerator(Enumerator).Callback(CallbackAction, CallbackCase.Cancel).Complete());
            Assert.IsTrue(result);
        }

        [Test]
        public void CancelledRoutineShouldInvokeCallbackIfCallbackCaseIsNotComplete()
        {
            LogAssert.ignoreFailingMessages = true;
            var result = false;
            void CallbackAction() => result = true;

            IEnumerator Enumerator(RoutineControl routineControl)
            {
                yield return null;
                routineControl.GetRoutine().Cancel();
            }

            Assert.Throws<InvalidOperationException>(() =>
                Routine.ByEnumerator(Enumerator).Callback(CallbackAction, CallbackCase.NotComplete).Complete());
            Assert.IsTrue(result);
        }

        [Test]
        public void CancelledRoutineShouldInvokeCallbackIfCallbackCaseIsAny()
        {
            LogAssert.ignoreFailingMessages = true;
            var result = false;
            void CallbackAction() => result = true;

            IEnumerator Enumerator(RoutineControl routineControl)
            {
                yield return null;
                routineControl.GetRoutine().Cancel();
            }

            Assert.Throws<InvalidOperationException>(() =>
                Routine.ByEnumerator(Enumerator).Callback(CallbackAction, CallbackCase.Any).Complete());
            Assert.IsTrue(result);
        }

        private class TestRoutine : Routine
        {
            protected override IEnumerator RoutineUpdate() => null;
        }
    }
}