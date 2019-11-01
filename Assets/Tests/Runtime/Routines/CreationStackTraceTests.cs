using System;
using System.Collections;
using System.Diagnostics;
using System.Text.RegularExpressions;
using NUnit.Framework;
using Omega.Package;
using UnityEngine;
using UnityEngine.TestTools;
using Debug = UnityEngine.Debug;

namespace Omega.Routines.Tests
{
    public class CreationStackTraceTests
    {
        [UnityTest]
        public IEnumerator RoutineExceptionShouldContainCreationStackTraceTest()
        {
            var exception = new Exception("");
            string messageFromLog = null;

            Application.logMessageReceived += (logMessage, logTrace, logType) => { messageFromLog = logMessage; };

            var targetStackTrace = new StackTrace(1, true).ToString();
            var routine = Routine.Task(() => throw exception)
                .CreationStackTrace();

            LogAssert.Expect(LogType.Error,
                new Regex("."));

            yield return routine;

            Assert.True(routine.IsError);
            Assert.IsNotEmpty(messageFromLog);
            Assert.True(messageFromLog.Contains(targetStackTrace));
        }
    }
}