using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Omega.Routines
{
    internal sealed class WaitAsyncOperationRoutine<TAsyncOperation, TResult> 
        : Routine<TResult>, IProgressRoutineProvider
        where TAsyncOperation : AsyncOperation
    {
        private TAsyncOperation _asyncOperation;
        private Func<TAsyncOperation, TResult> _resultSelector;
        private bool? _canBeForcedCompletion;

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

        protected override void OnForcedComplete()
        {
            if (!_canBeForcedCompletion.HasValue)
                _canBeForcedCompletion = _asyncOperation.CanBeForceComplete();
            
            if(!_canBeForcedCompletion.Value)
                throw new InvalidOperationException("AsyncOperation can not be forced complete");
        }

        public float GetProgress()
        {
            return _asyncOperation.progress;
        }
    }
    
    internal sealed class WaitAsyncOperationRoutine : Routine, IProgressRoutineProvider
    {
        private AsyncOperation _asyncOperation;
        private bool? _canBeForcedCompletion;

        public WaitAsyncOperationRoutine(AsyncOperation asyncOperation, bool? canBeForcedCompletion = false)
        {
            _asyncOperation = asyncOperation ?? throw new ArgumentNullException(nameof(asyncOperation));
            _canBeForcedCompletion = canBeForcedCompletion;
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

        protected override void OnForcedComplete()
        {
            if (!_canBeForcedCompletion.HasValue)
                _canBeForcedCompletion = _asyncOperation.CanBeForceComplete();
            
            if(!_canBeForcedCompletion.Value)
                throw new InvalidOperationException("AsyncOperation can not be forced complete");
        }
    }
}