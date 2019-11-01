using System;
using System.Collections;
using JetBrains.Annotations;

namespace Omega.Routines
{
    public class ByEnumeratorRoutine : Routine
    {
        [CanBeNull] private IEnumerator _enumerator;

        internal ByEnumeratorRoutine([CanBeNull] IEnumerator enumerator)
        {
            _enumerator = enumerator;
        }

        internal ByEnumeratorRoutine(Func<RoutineControl, IEnumerator> enumeratorGetter)
        {
            _enumerator = enumeratorGetter(new RoutineControl(this));
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