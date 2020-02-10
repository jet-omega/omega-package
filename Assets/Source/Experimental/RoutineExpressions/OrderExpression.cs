using System.Collections;
using System.Linq;

namespace Omega.Routines.Experimental
{
    internal class OrderExpression : IRoutineExpression
    {
        private readonly IRoutineExpression[] _expressions;

        public OrderExpression(IRoutineExpression[] expressions)
        {
            _expressions = expressions.ToArray();
        }

        public Routine ToRoutine()
        {
            IEnumerator Order(IRoutineExpression[] expressions)
            {
                foreach (var expression in expressions)
                    yield return expression.ToRoutine();
            }

            return Routine.ByEnumerator(Order(_expressions));
        }
    }
}