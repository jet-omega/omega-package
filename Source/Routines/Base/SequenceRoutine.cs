using System.Collections;

namespace Omega.Routines
{
    public sealed class SequenceRoutine : Routine, IProgressRoutineProvider
    {
        private readonly Routine[] _routines;
        private int _currentRoutine = -1;

        public SequenceRoutine(params Routine[] routines) => _routines = routines;

        protected override IEnumerator RoutineUpdate()
        {
            for (_currentRoutine = 0; _currentRoutine < _routines.Length; _currentRoutine++)
                yield return _routines[_currentRoutine];
        }

        protected override void OnCancel() => _routines[_currentRoutine].Cancel();

        public float GetProgress()
        {
            if (_currentRoutine == _routines.Length)
                return 1f;

            var progressPerRoutine = 1f / _routines.Length;
            return _currentRoutine >= 0
                ? _currentRoutine * progressPerRoutine +
                  RoutineUtilities.GetProgressFromRoutine(_routines[_currentRoutine])
                : 0;
        }
    }
}