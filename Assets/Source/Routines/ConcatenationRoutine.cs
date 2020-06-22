using System.Collections;
using System.Collections.Generic;

namespace Omega.Routines
{
    internal sealed class ConcatenationRoutine : Routine, IProgressRoutineProvider
    {
        private List<Routine> _routines;
        private GroupRoutine _finalGroupRoutine;

        public ConcatenationRoutine(IEnumerable<Routine> routines)
        {
            _routines = new List<Routine>(routines);
        }

        public ConcatenationRoutine(params Routine[] routines)
        {
            _routines = new List<Routine>(routines.Length);
            foreach (var routine in routines)
                _routines.Add(routine);
        }

        private ConcatenationRoutine(List<Routine> routines)
        {
            _routines = routines;
        }

        public ConcatenationRoutine Add(Routine routine)
        {
            var routinesList = new List<Routine>(_routines.Count + 1);
            routinesList.AddRange(_routines);
            routinesList.Add(routine);
            
            return new ConcatenationRoutine(routinesList);
        }

        public ConcatenationRoutine Add(ConcatenationRoutine routine)
        {
            var routinesList = new List<Routine>(_routines.Count + routine._routines.Count);
            routinesList.AddRange(_routines);
            routinesList.AddRange(routine._routines);
            
            return new ConcatenationRoutine(routinesList);
        }
        
        protected override IEnumerator RoutineUpdate()
        {
            yield return _finalGroupRoutine = new GroupRoutine(_routines);
        }

        public float GetProgress()
        {
            return _finalGroupRoutine?.GetProgress() ?? 0;
        }
    }
}