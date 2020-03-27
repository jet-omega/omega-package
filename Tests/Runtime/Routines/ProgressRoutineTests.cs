using NUnit.Framework;
using UnityEngine;

namespace Omega.Routines.Tests
{
    public class ProgressRoutineTests
    {
        [Test]
        public void SimpleRoutineNotImplementedProgressProvidedTest()
        {
            var progress = -1f;
            var routine = new EmptyTestRoutine(EmptyTestRoutine.SkipMode.OnceSkipEnumerator);
            var e = routine.OnProgress(p => progress = p);
            e.Complete();
            
            Assert.AreEqual(1, progress);
        }
        [Test]
        public void MomentumRoutineNotImplementedProgressProvidedTest()
        {
            var progress = -1f;
            var routine = new EmptyTestRoutine(EmptyTestRoutine.SkipMode.EmptyEnumerator);
            var e = routine.OnProgress(p => progress = p);
            e.Complete();
            
            Assert.AreEqual(1, progress);
        }
        [Test]
        public void EmptyRoutineNotImplementedProgressProvidedTest()
        {
            var progress = -1f;
            var routine = new EmptyTestRoutine(EmptyTestRoutine.SkipMode.NoEnumerator);
            var e = routine.OnProgress(p => progress = p);
            e.Complete();
            
            Assert.AreEqual(1, progress);
        }
    }
}