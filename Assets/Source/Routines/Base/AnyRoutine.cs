using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Omega.Routines
{
    public sealed class AnyRoutine : Routine, IProgressRoutineProvider
    {
        private readonly Routine[] _routines;

        internal AnyRoutine(IEnumerable<Routine> routines)
        {
            _routines = routines.ToArray();
        }

        internal AnyRoutine(params Routine[] routines) : this((IEnumerable<Routine>) routines)
        {
        }

        protected override IEnumerator RoutineUpdate()
        {
            bool MoveNextAll()
            {
                return _routines
                    .Cast<IEnumerator>()
                    .All(enumerator => enumerator.MoveNext());
            }

            while (MoveNextAll())
                yield return null;
        }
        
        public AnyRoutine Routines(out Routine[] routines)
        {
            routines = _routines.ToArray();
            return this;
        }

        public float GetProgress()
        {
            return _routines.Max(RoutineUtilities.GetProgressFromRoutine);
        }
    }
}
