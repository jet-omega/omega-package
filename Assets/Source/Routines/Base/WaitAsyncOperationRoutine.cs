using System;
using System.Collections;
using UnityEngine;

namespace Omega.Routines
{
    internal sealed class WaitAsyncOperationRoutine<TAsyncOperation, TResult> : Routine<TResult>, IProgressRoutineProvider
        where TAsyncOperation : AsyncOperation
    {
        private TAsyncOperation _asyncOperation;
        private Func<TAsyncOperation, TResult> _resultSelector;

        public WaitAsyncOperationRoutine(TAsyncOperation asyncOperation, Func<TAsyncOperation, TResult> resultSelector)
        {
            _asyncOperation = asyncOperation ?? throw new ArgumentNullException(nameof(asyncOperation));
            _resultSelector = resultSelector ?? throw new ArgumentNullException(nameof(resultSelector));
        }

        protected override IEnumerator RoutineUpdate()
        {
            while (!_asyncOperation.isDone)
                yield return null;

            var result = _resultSelector(_asyncOperation);
            SetResult(result);
        }

        public float GetProgress()
        {
            return _asyncOperation.progress;
        }
    }
    
    internal sealed class WaitAsyncOperationRoutine : Routine, IProgressRoutineProvider
    {
        private AsyncOperation _asyncOperation;

        public WaitAsyncOperationRoutine(AsyncOperation asyncOperation)
        {
            _asyncOperation = asyncOperation ?? throw new ArgumentNullException(nameof(asyncOperation));
        }

        protected override IEnumerator RoutineUpdate()
        {
            while (!_asyncOperation.isDone)
                yield return null;
        }

        public float GetProgress()
        {
            return _asyncOperation.progress;
        }
    }
}