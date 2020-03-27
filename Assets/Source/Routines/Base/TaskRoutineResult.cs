using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;

namespace Omega.Routines
{
    public sealed class TaskRoutine<TResult> : Routine<TResult>
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly Func<CancellationToken, TResult> _action;

        public TaskRoutine(Func<TResult> action)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));

            _cancellationTokenSource = new CancellationTokenSource();
            _action = _ => action();
        }

        public TaskRoutine(Func<CancellationToken, TResult> action)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _action = action ?? throw new ArgumentNullException(nameof(action));
        }

        protected override IEnumerator RoutineUpdate()
        {
            var token = _cancellationTokenSource.Token;
            var task = new Task<TResult>(() => _action(token));
            task.Start();

            while (task.Status == TaskStatus.Running || task.Status == TaskStatus.WaitingForActivation ||
                   task.Status == TaskStatus.WaitingToRun)
                yield return null;

            if (task.IsFaulted)
                throw task.Exception;

            SetResult(task.Result);
        }
        
        protected override void OnCancel()
        {
            _cancellationTokenSource.Cancel();
        }
        
        public TaskRoutine<TResult> StartTask()
        {
            ((IEnumerator) this).MoveNext();
            return this;
        }
    }
}