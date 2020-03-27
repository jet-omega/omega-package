using System;
using System.Collections;
using JetBrains.Annotations;

namespace Omega.Routines
{
    public class ByEnumeratorRoutine : Routine, IProgressRoutineProvider
    {
        [CanBeNull] private IEnumerator _enumerator;
        private float _progress;
        internal Action OnCancelation;
        internal Action OnForceComplete;
            
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

        internal void SetProgress(float progress)
        {
            _progress = progress;
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