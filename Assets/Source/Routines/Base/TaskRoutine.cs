using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Omega.Routines
{
    public class TaskRoutine : Routine
    {
        private readonly Action<CancellationToken> _action;
        private readonly CancellationTokenSource _cancellationTokenSource;

        internal TaskRoutine(Action action)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));

            _cancellationTokenSource = new CancellationTokenSource();
            _action = _ => action();
        }

        internal TaskRoutine(Action<CancellationToken> action)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _action = action ?? throw new ArgumentNullException(nameof(action));
        }

        protected override IEnumerator RoutineUpdate()
        {
            var token = _cancellationTokenSource.Token;
            var task = new Task(() => _action(token), token);
            task.Start();

            while (task.Status == TaskStatus.Running || task.Status == TaskStatus.WaitingForActivation ||
                   task.Status == TaskStatus.WaitingToRun)
                yield return null;

            if (task.IsFaulted)
                throw task.Exception;
        }

        protected override void OnCancel()
        {
            _cancellationTokenSource.Cancel();
        }

        public TaskRoutine StartTask()
        {
            ((IEnumerator) this).MoveNext();
            return this;
        }
    }
}