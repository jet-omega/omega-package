using System;
using System.Collections;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Omega.Routines
{
    public class TaskRoutine : Routine
    {
        private readonly Action _action;
        private string _initialInvokeStackTrace;

        internal TaskRoutine(Action action)
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

            if (task.IsFaulted)
                throw task.Exception.InnerException;
        }

        public TaskRoutine StartTask()
        {
            ((IEnumerator) this).MoveNext();
            return this;
        }
    }
}