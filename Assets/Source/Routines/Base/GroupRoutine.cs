using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Omega.Routines
{
    public sealed class GroupRoutine : Routine
    {
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

        public GroupRoutine Routines(out Routine[] routines)
        {
            routines = _routines.ToArray();
            return this;
        }
    }
}