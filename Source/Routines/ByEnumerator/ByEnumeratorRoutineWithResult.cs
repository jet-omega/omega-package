using System;
using System.Collections;
using JetBrains.Annotations;

namespace Omega.Routines
{
    public class ByEnumeratorRoutine<TResult> : Routine<TResult>, IProgressRoutineProvider
    {
        [CanBeNull] private IEnumerator _enumerator;
        internal Action OnCancelation;
        internal Action OnForceComplete;
        private float _progress;

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

        protected override void OnCancel()
        {
            OnCancelation?.Invoke();
            base.OnCancel();
        }

        protected override void OnForcedComplete()
        {
            OnForceComplete?.Invoke();
            base.OnForcedComplete();
        }

        internal void SetProgress(float progress)
        {
            _progress = progress;
        }
        
        public IEnumerator GetEnumerator()
        {
            return _enumerator;
        }

        public float GetProgress()
        {
            return IsComplete ? 1 : _progress;
        }
    }
}