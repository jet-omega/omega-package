using System;
using System.Collections;
using JetBrains.Annotations;

namespace Omega.Routines
{
    public class ByEnumeratorRoutine<TResult> : Routine<TResult>
    {
        [CanBeNull] private IEnumerator _enumerator;

        internal ByEnumeratorRoutine(Func<RoutineControl<TResult>, IEnumerator> enumeratorGetter)
        {
            _enumerator = enumeratorGetter(new RoutineControl<TResult>(this));
        }

        protected sealed override IEnumerator RoutineUpdate()
        {
            if (_enumerator == null)
                yield break;

            while (_enumerator.MoveNext())
                yield return _enumerator.Current;
        }
        
        public IEnumerator GetEnumerator()
        {
            return _enumerator;
        }
    }
}