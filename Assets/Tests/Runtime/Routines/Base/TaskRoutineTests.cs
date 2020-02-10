using System.Collections;
using System.Threading;
using NUnit.Framework;
using Omega.Package;
using UnityEngine;
using UnityEngine.TestTools;

namespace Omega.Routines.Tests
{
    public class TaskRoutineTests
    {
        [Test]
        public void RoutinePropertyIsNotStartedShouldReturnFalseWhenCallStartThreadTest()
        {
            var routine = Routine.Task(() => Thread.Sleep(15)).StartTask();
            Assert.False(routine.IsNotStarted);
        }

        [UnityTest]
        public IEnumerator CancelTaskRoutineShouldCancelTaskYetTest()
        {
            bool? cancellationExit = null;

            var taskRoutine = Routine.Task(token =>
            {
                var timeMeter = TimeMeter.New();
                while (timeMeter.ToSeconds() < 1)
                {
                    Thread.Sleep(15);
                    if (token.IsCancellationRequested)
                    {
                        cancellationExit = true;
                        return;
                    }
                }

                cancellationExit = false;
            }).InBackground();

            yield return Routine.Delay(0.1f);

            taskRoutine.Cancel();

            // Отклик на отмену задачи ~15 мс ( Thread.Sleep(15) ), поэтому нужно немного подождать
            yield return Routine.Delay(0.03f);
            
            Assert.NotNull(cancellationExit);
            Assert.True(cancellationExit);
        }
    }
}