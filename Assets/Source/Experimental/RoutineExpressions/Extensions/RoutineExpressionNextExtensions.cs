using System;
using System.Collections;

namespace Omega.Routines.Experimental
{
    internal static class RoutineExpressionNextExtensions
    {
        public static IRoutineExpression Next(this IRoutineExpression self, IRoutineExpression next)
        {
            return RoutineExpression.Order(self, next);
        }

        public static IRoutineExpression<T> Next<T>(this IRoutineExpression self, IRoutineExpression<T> next)
        {
            IEnumerator Enumerator(RoutineControl<T> @this)
            {
                yield return self.ToRoutine();
                yield return next.ToRoutine().Result(out var result);
                @this.SetResult(result);
            }
            
            return RoutineExpression.From<T>(Enumerator);
        }

        public static IRoutineExpression Next<T>(this IRoutineExpression<T> self, Func<T, Routine> toRoutine)
        {
            IEnumerator Enumerator(RoutineControl @this)
            {
                yield return self.ToRoutine().Result(out var result);
                yield return toRoutine(result);
            }

            return RoutineExpression.From(Enumerator);
        }


        public static IRoutineExpression<TOut> Next<TIn, TOut>(this IRoutineExpression<TIn> self,
            Func<TIn, Routine<TOut>> toRoutine)
        {
            IEnumerator Enumerator(RoutineControl<TOut> @this)
            {
                yield return self.ToRoutine().Result(out var result);
                yield return toRoutine(result).Result(out var outResult);
                @this.SetResult(outResult);
            }

            return RoutineExpression.From<TOut>(Enumerator);
        }
    }
}