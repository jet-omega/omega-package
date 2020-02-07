using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Omega.Routines
{
    public static class AsyncOperationExtensions
    {
        private static readonly HashSet<Type> ForcibleAsyncOperationTypes = new HashSet<Type>()
        {
            typeof(UnityWebRequestAsyncOperation)
        };

        public static bool CanBeForceComplete(this AsyncOperation self)
        {
            if(self is null)
                throw new NullReferenceException(nameof(self));
            
            var asyncOperationType = self.GetType();
            return ForcibleAsyncOperationTypes.Contains(asyncOperationType);
        }
        
        public static Routine AsRoutine<TAsyncOperation>(this TAsyncOperation self, out TAsyncOperation asyncOperation)
            where TAsyncOperation : AsyncOperation
        {
            var routine = new WaitAsyncOperationRoutine(self);
            asyncOperation = self;
            return routine;
        }
        
        public static Routine AsRoutine<TAsyncOperation>(this TAsyncOperation self)
            where TAsyncOperation : AsyncOperation
        {
            var routine = new WaitAsyncOperationRoutine(self);
            return routine;
        }
        
        public static Routine<TResult> AsRoutine<TAsyncOperation, TResult>(this TAsyncOperation self, Func<TAsyncOperation, TResult> selector)
            where TAsyncOperation : AsyncOperation
        {
            var routine = new WaitAsyncOperationRoutine<TAsyncOperation, TResult>(self, selector);
            return routine;
        }

        public static TAsyncOperation GetSelf<TAsyncOperation>(this TAsyncOperation self, out TAsyncOperation outSelf)
            where TAsyncOperation : AsyncOperation
        {
            return outSelf = self;
        }
    }
}