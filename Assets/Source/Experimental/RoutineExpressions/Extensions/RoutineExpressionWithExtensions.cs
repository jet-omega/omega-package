using System.Collections;

namespace Omega.Routines.Experimental
{
    public static class RoutineExpressionWithExtensions
    {
        public static IRoutineExpression With(this IRoutineExpression self, IRoutineExpression with)
        {
            IEnumerator Enumerator(RoutineControl @this)
            {
                yield return self.ToRoutine() + with.ToRoutine();
            }

            return RoutineExpression.From(Enumerator);
        }

        public static IRoutineExpression<T> With<T>(this IRoutineExpression<T> self, IRoutineExpression with)
        {
            IEnumerator Enumerator(RoutineControl<T> @this)
            {
                yield return self.ToRoutine().Result(out var result) + with.ToRoutine();
                @this.SetResult(result);
            }

            return RoutineExpression.From<T>(Enumerator);
        }
    }
}