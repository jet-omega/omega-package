using System;
using UnityEngine;

namespace Omega.Routines
{
    public static class AsyncOperationExtensions
    {
        public static Routine AsRoutine<TAsyncOperation>(this TAsyncOperation self, out TAsyncOperation asyncOperation)
            where TAsyncOperation : AsyncOperation
        {
            var routine = new WaitAsyncOperationRoutine(self);
            asyncOperation = self;
            return routine;
        }
        
        public static Routine<TResult> AsRoutine<TAsyncOperation, TResult>(this TAsyncOperation self, Func<TAsyncOperation, TResult> selector)
            where TAsyncOperation : AsyncOperation
        {
            var routine = new WaitAsyncOperationRoutine<TAsyncOperation, TResult>(self, selector);
            return routine;
        }
    }
}