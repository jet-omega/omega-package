using System;
using System.Collections;
using System.Threading.Tasks;

namespace Omega.Experimental.Routines
{
    public class OtherThreadRoutine : Routine
    {
        private readonly Action _action;

        public OtherThreadRoutine(Action action)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
        }

        protected override IEnumerator RoutineUpdate()
        {
            var task = new Task(_action);
            task.Start();

            while (task.Status == TaskStatus.Running || task.Status == TaskStatus.WaitingForActivation ||
                   task.Status == TaskStatus.WaitingToRun)
                yield return null;
            
            if(task.IsFaulted)
                SetException(task.Exception);
        }
    }
}