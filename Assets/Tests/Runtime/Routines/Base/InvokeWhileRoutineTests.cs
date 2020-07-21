using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Omega.Routines.Tests
{
    public class InvokeWhileRoutineTests
    {
        [UnityTest]
        public IEnumerator InvokeWhileExecutesOnlyWhileTargetExecutes()
        {
            var delay = 1;
            var targetRoutine = Routine.Delay(delay);
            var startTime = DateTime.Now;
            var lastInvokeTime = startTime;
            yield return Routine.InvokeWhile(targetRoutine, () => { lastInvokeTime = DateTime.Now; });
            Assert.LessOrEqual((lastInvokeTime - startTime).Seconds, delay);
        }
    }
}