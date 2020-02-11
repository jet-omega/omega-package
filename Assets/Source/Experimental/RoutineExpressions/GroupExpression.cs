
using System.Linq;

namespace Omega.Routines.Experimental
{
    internal class GroupExpression : IRoutineExpression
    {
        private readonly IRoutineExpression[] _expressions;

        public GroupExpression(IRoutineExpression[] expressions)
        {
            _expressions = expressions;
        }

        public Routine ToRoutine()
        {
            var routines = _expressions.Select(e => e.ToRoutine()).ToArray();
            return Routine.WhenAll(routines);
        }
    }
}