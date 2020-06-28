using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Omega.Routines
{
    [Obsolete("Use WhenAllRoutine")]
    public sealed class GroupRoutine : Routine, IProgressRoutineProvider
    {
        private readonly WhenAllRoutine _whenAllRoutine;
        
        internal GroupRoutine(IEnumerable<Routine> routines)
        {
            _whenAllRoutine = new WhenAllRoutine(routines);
        }

        internal GroupRoutine(params Routine[] routines)
            : this((IEnumerable<Routine>) routines)
        {
        }
            
        protected override IEnumerator RoutineUpdate()
        {
            while (((IEnumerator) _whenAllRoutine).MoveNext())
                yield return null;
        }

        public float GetProgress() => _whenAllRoutine.GetProgress();
    }
    
    public sealed class WhenAllRoutine : Routine, IProgressRoutineProvider
    {
        private readonly Routine[] _routines;

        internal WhenAllRoutine(IEnumerable<Routine> routines)
        {
            _routines = routines.ToArray();
        }

        internal WhenAllRoutine(params Routine[] routines)
            : this((IEnumerable<Routine>) routines)
        {
        }

        protected override IEnumerator RoutineUpdate()
        {
            bool MoveNextAll()
            {
                var flag = false;
                foreach (var routine in _routines)
                {
                    var enumerator = (IEnumerator) routine;
                    flag |= enumerator.MoveNext();
                }

                return flag;
            }

            while (MoveNextAll())
                yield return null;
        }

        public WhenAllRoutine Routines(out Routine[] routines)
        {
            routines = _routines.ToArray();
            return this;
        }

        public float GetProgress()
        {
            var totalCount = _routines.Length;
            var progressPerRoutine = 1f / totalCount;

            var totalProgress = 0f;
            foreach (var routine in _routines)
            {
                if (routine.IsComplete)
                    totalProgress += progressPerRoutine;
                else if (routine is IProgressRoutineProvider progressRoutineProvider)
                {
                    var routineProgress = progressRoutineProvider.GetProgress();
                    totalProgress += routineProgress * progressPerRoutine;
                }
            }

            return totalProgress;
        }
    }
}