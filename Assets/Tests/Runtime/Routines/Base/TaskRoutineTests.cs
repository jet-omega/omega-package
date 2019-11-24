using System.Threading;
using NUnit.Framework;

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
    }
}