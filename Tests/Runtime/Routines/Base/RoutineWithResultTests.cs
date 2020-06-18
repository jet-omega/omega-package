using System;
using NUnit.Framework;

namespace Omega.Routines.Tests
{
    public class RoutineWithResultTests
    {
        [Test]
        public void RoutineWithResultShouldProcessCallbackWhenRoutineMoveNext()
        {
            bool flag = false;
            var routine = Routine.GetWithResult(new object())
                .Callback(e => flag = true);

            routine.Complete();
            
            Assert.True(flag);
        }
    }
}