using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Omega.Routines
{
    public sealed class GroupRoutine : Routine
    {
        private IEnumerator[] _processingRoutines;
        private readonly Routine[] _routines;

        public GroupRoutine(IEnumerable<Routine> routines)
        {
            _routines = routines.ToArray();
        }

        public GroupRoutine(params Routine[] routines)
            : this((IEnumerable<Routine>) routines)
        {
        }

        protected override IEnumerator RoutineUpdate()
        {
            bool MoveNextAll()
            {
                bool flag = false;
                for (int i = 0; i < _processingRoutines.Length; i++)
                    flag |= _processingRoutines[i].MoveNext();

                return flag;
            }

            if (_processingRoutines == null)
                _processingRoutines = CreateEnumeratorsFromRoutines(_routines);

            while (MoveNextAll())
                yield return null;
        }

        public GroupRoutine ParallelsRoutines(out Routine[] routines)
        {
            routines = _processingRoutines.Cast<Routine>().ToArray();
            return this;
        }

        //TODO: OPTIMIZE
        private IEnumerator[] CreateEnumeratorsFromRoutines(Routine[] routines) =>
            routines
                .Cast<IEnumerator>()
                .ToArray();
    }
}