using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Omega.Routines
{
    public sealed class GroupRoutine : Routine
    {
        private IEnumerator[] _processingRoutines;
        private readonly Routine[] _routines;

        internal GroupRoutine(IEnumerable<Routine> routines)
        {
            _routines = routines.ToArray();
        }

        internal GroupRoutine(params Routine[] routines)
            : this((IEnumerable<Routine>) routines)
        {
        }

        protected override IEnumerator RoutineUpdate()
        {
            bool MoveNextAll()
            {
                var flag = false;
                for (int i = 0; i < _processingRoutines.Length; i++)
                    flag |= _processingRoutines[i].MoveNext();

                return flag;
            }

            if (_processingRoutines == null)
                _processingRoutines = CastToEnumerators(_routines);

            while (MoveNextAll())
                yield return null;
        }

        public GroupRoutine Routines(out Routine[] routines)
        {
            routines = _routines.ToArray();
            return this;
        }

        private IEnumerator[] CastToEnumerators(Routine[] routines)
        {
            var processingResult = new IEnumerator[routines.Length];
            for (int i = 0; i < processingResult.Length; i++)
                processingResult[i] = routines[i];
            return processingResult;
        }
    }
}