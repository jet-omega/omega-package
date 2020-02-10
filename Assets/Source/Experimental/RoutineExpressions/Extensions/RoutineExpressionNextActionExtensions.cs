using System;
using System.Collections;

namespace Omega.Routines.Experimental
{
    public static class RoutineExpressionNextActionExtensions
    {
        public static IRoutineExpression NextAction(this IRoutineExpression self, Action action)
        {
            var actionExpression = RoutineExpression.Action(action);
            return self.Next(actionExpression);
        }

        public static IRoutineExpression NextAction<TIn>(this IRoutineExpression<TIn> self, Action<TIn> action)
        {
            IEnumerator Enumerator(RoutineControl @this)
            {
                yield return self.ToRoutine().Result(out var result);
                yield return Routine.ByAction(() => action(result));
            }

            return RoutineExpression.FromRoutine(() => Routine.ByEnumerator(Enumerator));
        }

        public static IRoutineExpression<TOut> NextAction<TOut>(this IRoutineExpression self, Func<TOut> action)
        {
            var actionExpression = RoutineExpression.Action(action);
            return self.Next(actionExpression);
        }

        public static IRoutineExpression<TOut> NextAction<TIn, TOut>(this IRoutineExpression<TIn> self,
            Func<TIn, TOut> action)
        {
            IEnumerator Enumerator(RoutineControl<TOut> @this)
            {
                yield return self.ToRoutine().Result(out var result);
                yield return Routine.ByAction(() => action(result)).Result(out var finalResult);
                @this.SetResult(finalResult);
            }

            return RoutineExpression.FromRoutine(() => Routine.ByEnumerator<TOut>(Enumerator));
        }
    }
}