using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Omega.Routines.Tests.Continuation
{
    public class CatchTests
    {
        [UnityTest]
        public IEnumerator CancelledRoutineShouldStopParentRoutineByDefaultTest()
        {
            bool flag = false;

            IEnumerator CancelledRoutine(RoutineControl @this)
            {
                @this.GetRoutine().Cancel();
                yield break;
            }

            yield return Routine.Sequence(
                Routine.ByAction(() => flag = true).SetName("flag setup"),
                Routine.ByEnumerator(CancelledRoutine).SetName("cancelled routine"),
                Routine.ByAction(Assert.Fail)).SetName("action with fail");
            
            Assert.True(flag);
        }    
    }
}