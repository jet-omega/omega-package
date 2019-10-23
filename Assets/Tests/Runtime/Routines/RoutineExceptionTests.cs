using System;
using System.Collections;
using System.Text.RegularExpressions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Omega.Experimental.Routines.Tests
{
    public class RoutineExceptionTests
    {
        [UnityTest]
        public IEnumerator RoutineShouldSetupErrorStateWhenThrowException()
        {
            var keyMassage =
                $"This exception was thrown specifically for the unit test ({nameof(RoutineShouldSetupErrorStateWhenThrowException)})";

            LogAssert.ignoreFailingMessages = true;
            yield return Routine.Task(() => throw new Exception(keyMassage))
                .GetRoutine(out var routine);
            LogAssert.ignoreFailingMessages = false;

            Assert.True(routine.IsError);
            Assert.AreEqual(keyMassage, routine.Exception.Message);
        }
    }
}