using System;
using NUnit.Framework;

namespace Omega.Routines.Tests
{
    public class FromResultTests
    {
        [Test]
        public void FromResultShouldProcessCallbackWhenRoutineMoveNext()
        {
            bool flag = false;
            var routine = Routine.FromResult(new object())
                .Callback(e => flag = true);

            routine.Complete();
            
            Assert.True(flag);
        }
    }
}