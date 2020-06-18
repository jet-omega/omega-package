using NUnit.Framework;

namespace Omega.Routines.Tests
{
    public class CompletedRoutineTests
    {
        [Test]
        public void GetCompletedRoutineIsCompleteTest()
        {
            var routine = Routine.GetCompleted();
            Assert.True(routine.IsComplete);
        }
    }
}