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
            yield return Routine.ByAction(() => flags[0] = true) + Routine.ByAction(() => flags[1] = true) +
                         Routine.ByAction(() => flags[2] = true);

            Assert.True(flags.All(e => e));
        }
    }
}