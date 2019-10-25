using System.Collections;
using System.Threading;
using NUnit.Framework;

namespace Omega.Routines.Tests
{
    public class RoutineStatusTests
    {
        [Test]
        public void RoutinePropertyIsNotStartedShouldBeReturnTrueWhenRoutineIsNotStartedTest()
        {
            var routine = Routine.Task(() => Thread.Sleep(15));
            Assert.True(routine.IsNotStarted);
        }
    }
}