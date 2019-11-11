using System;
using System.Collections;
using System.Threading.Tasks;

namespace Omega.Routines
{
    public sealed class TaskRoutine<TResult> : Routine<TResult>
    {
        private readonly Func<TResult> _action;

        public TaskRoutine(Func<TResult> action)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
        }

        protected override IEnumerator RoutineUpdate()
        {
            var task = new Task<TResult>(_action);
            task.Start();

            while (task.Status == TaskStatus.Running || task.Status == TaskStatus.WaitingForActivation ||
                   task.Status == TaskStatus.WaitingToRun)
                yield return null;

            if (task.IsFaulted) 
                throw task.Exception.InnerException;
            
            SetResult(task.Result);
        }
    }
}