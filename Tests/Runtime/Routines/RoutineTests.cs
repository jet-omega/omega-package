using System;
using System.Collections;
using System.Text.RegularExpressions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Omega.Routines.Tests
{
    public class RoutineTests
    {
        [Test]
        public void RoutineShouldThrowExceptionIfNestedRoutineHaveErrorTest()
        {
            var routineWithError = Routine.ByAction(() => throw new Exception("Its test exception"));
            LogAssert.Expect(LogType.Error, new Regex("."));
            routineWithError.Complete();
            
            
            IEnumerator TestRoutine(RoutineControl<int> control)
            {
                yield return routineWithError;
                control.SetResult(50);
            }

            var routine = Routine.ByEnumerator<int>(TestRoutine);
            LogAssert.Expect(LogType.Error, new Regex("."));
            routine.Complete();
            Assert.True(routine.IsError);
        }
    }
}