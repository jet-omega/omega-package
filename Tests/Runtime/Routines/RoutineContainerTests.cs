using System;
using NUnit.Framework;

namespace Omega.Routines.Tests
{
    public class RoutineContainerTests
    {
        [Test]
        public void ImplicitOperatorShouldReturnResultTest()
        {
            var routine = Routine.FromResult(DateTime.UtcNow.Ticks);
            long resultFromRoutine =  routine.Result(out var result).WaitResult();
            long resultFromContainer = result;
            
            Assert.AreEqual(resultFromContainer, resultFromRoutine);
            
        }
    }
}