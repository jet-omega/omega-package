using System.Collections;
using NUnit.Framework;
using Omega.Routines.IO;
using UnityEngine;
using UnityEngine.TestTools;

namespace Omega.Routines.Tests
{
    public class ConcatenationRoutinesTests
    {
        [UnityTest]
        public IEnumerator OperatorPlusShouldCreateGroupWithTwoRoutines()
        {
            var timeBeforeTest = Time.time;
            yield return Routine.Delay(0.050f) + Routine.Delay(0.080f) + Routine.Delay(0.080f);
            var deltaTime = Time.time - timeBeforeTest;
            Assert.Greater(0.1f, deltaTime);
        }
    }
}