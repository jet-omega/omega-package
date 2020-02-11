using System.Collections;
using System.Text.RegularExpressions;
using NUnit.Framework;
using Omega.Routines.Experimental;
using UnityEngine;
using UnityEngine.TestTools;

namespace Omega.Routines.Tests
{
    public class RoutineExpressionsTests
    {
        [UnityTest]
        public IEnumerator IfExpressionShouldExecuteTrueBranchTest()
        {
            bool wasInvokeFromTrueBranch = false;
            yield return RoutineExpression.New()
                .If(() => true,
                    () => wasInvokeFromTrueBranch = true,
                    Assert.Fail).ToRoutine();

            Assert.True(wasInvokeFromTrueBranch);
        }

        [UnityTest]
        public IEnumerator IfExpressionShouldExecuteFalseBranchTest()
        {
            bool wasInvokeFromFalseBranch = false;
            yield return RoutineExpression.New()
                .If(() => false,
                    Assert.Fail,
                    () => wasInvokeFromFalseBranch = true).ToRoutine();

            Assert.True(wasInvokeFromFalseBranch);
        }
        
#if UNITY_EDITOR
        [UnityTest]
        public IEnumerator NestedRoutineExpressionInRoutineIsErrorTest()
        {
            IEnumerator Enumerator()
            {
                yield return RoutineExpression.New();
            }

            LogAssert.Expect(LogType.Error, new Regex("."));
            yield return Routine.ByEnumerator(Enumerator());
        }
#endif
    }
}