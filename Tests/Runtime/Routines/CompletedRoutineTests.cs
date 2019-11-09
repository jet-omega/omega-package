using NUnit.Framework;

namespace Omega.Routines.Tests
{
    public class CompletedRoutineTests
    {
        [Test]
        public void FromCompletedShouldGetCompletedRoutine()
        {
            var routine = Routine.FromCompleted();
            Assert.True(routine.IsComplete);
        }
    }
}