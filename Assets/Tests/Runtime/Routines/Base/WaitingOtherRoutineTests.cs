using System;
using System.Collections;
using NUnit.Framework;
using Omega.Package;
using UnityEngine;
using UnityEngine.TestTools;

namespace Omega.Routines.Tests
{
    public class WaitingOtherRoutineTests
    {
        [UnityTest]
        public IEnumerator WaitingRoutineWhichIsProcessedFromOutsideTest()
        {
            var gameObject = new GameObject();
            var testMonoBehaviour = gameObject.AddComponent<TestMonoBehaviour>();
            
            var delay = TimeSpan.FromMilliseconds(75);
            var delayRoutine = Routine.Delay(delay);

            testMonoBehaviour.StartCoroutine(delayRoutine);

            var startTime = DateTime.UtcNow;
            yield return delayRoutine;
            var delta = DateTime.UtcNow - startTime;
            
            Utilities.Object.AutoDestroy(gameObject);
            
            Assert.GreaterOrEqual(delta, delay);
        }

        [UnityTest]
        public IEnumerator WaitingComplexRoutineWhichIsProcessedFromOutsideTest()
        {
            var gameObject = new GameObject();
            var testMonoBehaviour = gameObject.AddComponent<TestMonoBehaviour>();
            
            var delay = TimeSpan.FromMilliseconds(75);
            var delayRoutine = Routine.Delay(delay);
            var complexRoutine = TestRoutine.CreateComplex(5, delayRoutine);

            testMonoBehaviour.StartCoroutine(complexRoutine);

            var startTime = DateTime.UtcNow;
            yield return complexRoutine;
            var delta = DateTime.UtcNow - startTime;
            
            Utilities.Object.AutoDestroy(gameObject);
            
            Assert.GreaterOrEqual(delta, delay);
        }
        
        private class TestMonoBehaviour : MonoBehaviour
        {
        }

        private class TestRoutine : Routine
        {
            private IEnumerator _targetRoutine;

            public TestRoutine(IEnumerator targetRoutine)
            {
                _targetRoutine = targetRoutine;
            }

            protected override IEnumerator RoutineUpdate()
            {
                yield return _targetRoutine;
            }

            public static Routine CreateComplex(int level, Routine target)
            {
                if (level == 0)
                    return target;

                return new TestRoutine(CreateComplex(level - 1, target));
            }
        }
    }
}