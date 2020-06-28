using System.Collections;
using System.Linq;
using NUnit.Framework;
using Omega.Routines.IO;
using UnityEngine;
using UnityEngine.TestTools;

namespace Omega.Routines.Tests
{
    public class ConcatenationRoutinesTests
    {
        [UnityTest]
        public IEnumerator OperatorPlusShouldCreateGroupWithThreeRoutines()
        {
            var flags = new bool[3];
            yield return Routine.Action(() => flags[0] = true) + Routine.Action(() => flags[1] = true) +
                         Routine.Action(() => flags[2] = true);

            Assert.True(flags.All(e => e));
        }
    }
}