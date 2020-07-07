using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Omega.Routines.Tests
{
    public class RoutineProgressTests
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

            Assert.AreEqual(1f, progress);
        }

        [Test]
        public void RoutineProgressTest()
        {
            IEnumerator RoutineSteps(int steps, RoutineControl @this)
            {
                for (int i = 0; i < steps; i++)
                {
                    yield return null;
                    @this.SetProgress((i + 1) / (float) steps);
                }
            }

            var progressReference = 0f;
            var testSteps = 10;
            Assert.Greater(testSteps, 1);

            var routine = Routine.ByEnumerator(RoutineSteps, testSteps);
            routine.OnProgress(progress => progressReference = progress);
            for (int i = 0; i < testSteps; i++)
            {
                RoutineUtilities.OneStep(routine);
                var expectedProgress = (i) / (float) testSteps;
                Assert.AreEqual(expectedProgress, progressReference);
            }
        }

        [UnityTest]
        public IEnumerator NestedRoutineProgressTest()
        {
            IEnumerator NestedRoutineSteps(int steps, RoutineControl @this)
            {
                for (int i = 0; i < steps; i++)
                {
                    yield return null;
                    @this.SetProgress((i + 1) / (float) steps);
                }
            }

            var progressReference = 0f;
            var testSteps = 10;
            Assert.Greater(testSteps, 1);

            var nestedRoutine = Routine.ByEnumerator(NestedRoutineSteps, testSteps);

            IEnumerator RoutineSteps(RoutineControl @this)
            {
                yield return nestedRoutine.OnProgress(@this.SetProgress);
            }

            var routine = Routine.ByEnumerator(RoutineSteps);

            routine.OnProgress(p => progressReference = p);

            yield return routine;

            Assert.AreEqual(1, progressReference);
        }
    }
}