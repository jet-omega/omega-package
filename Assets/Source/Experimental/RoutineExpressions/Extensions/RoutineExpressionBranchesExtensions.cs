using System;
using System.Collections;

namespace Omega.Routines.Experimental
{
    public static class RoutineExpressionBranchesExtensions
    {
        public static IRoutineExpression If(this IRoutineExpression self, IRoutineExpression<bool> condition,
            IRoutineExpression trueBranch, IRoutineExpression falseBranch)
        {
            IEnumerator Enumerator(RoutineControl @this)
            {
                yield return self.ToRoutine();
                yield return condition.ToRoutine().Result(out var conditionResult);
                var branch = conditionResult ? trueBranch : falseBranch;
                
                if (branch != null)
                    yield return branch.ToRoutine();
            }

            return RoutineExpression.From(Enumerator);
        }

        public static IRoutineExpression If(this IRoutineExpression self, Func<bool> condition,
            IRoutineExpression trueBranch, IRoutineExpression falseBranch)
        {
            IEnumerator Enumerator(RoutineControl @this)
            {
                yield return self.ToRoutine();
                var conditionResult = condition.Invoke();
                var branch = conditionResult ? trueBranch : falseBranch;
                
                if (branch != null)
                    yield return branch.ToRoutine();
            }

            return RoutineExpression.From(Enumerator);
        }

        public static IRoutineExpression If<TIn>(this IRoutineExpression<TIn> self, Func<TIn, Routine<bool>> condition,
            IRoutineExpression trueBranch, IRoutineExpression falseBranch)
        {
            IEnumerator Enumerator(RoutineControl @this)
            {
                yield return self.ToRoutine().Result(out var result);
                yield return condition.Invoke(result).Result(out var conditionResult);
                var branch = conditionResult ? trueBranch : falseBranch;
                
                if (branch != null)
                    yield return branch.ToRoutine();
            }

            return RoutineExpression.From(Enumerator);
        }

        public static IRoutineExpression If<TIn>(this IRoutineExpression<TIn> self, Func<TIn, bool> condition,
            IRoutineExpression trueBranch, IRoutineExpression falseBranch)
        {
            IEnumerator Enumerator(RoutineControl @this)
            {
                yield return self.ToRoutine().Result(out var result);
                var conditionResult = condition.Invoke(result);
                var branch = conditionResult ? trueBranch : falseBranch;
                
                if (branch != null)
                    yield return branch.ToRoutine();
            }

            return RoutineExpression.From(Enumerator);
        }

        public static IRoutineExpression If<TIn>(this IRoutineExpression<TIn> self, Func<TIn, bool> condition,
            Action trueBranch, Action falseBranch)
        {
            IEnumerator Enumerator(RoutineControl @this)
            {
                yield return self.ToRoutine().Result(out var result);
                var conditionResult = condition.Invoke(result);
                (conditionResult ? trueBranch : falseBranch)?.Invoke();
            }

            return RoutineExpression.From(Enumerator);
        }

        public static IRoutineExpression If(this IRoutineExpression self, Func<bool> condition,
            Action trueBranch, Action falseBranch)
        {
            IEnumerator Enumerator(RoutineControl @this)
            {
                yield return self.ToRoutine();
                var conditionResult = condition.Invoke();
                (conditionResult ? trueBranch : falseBranch)?.Invoke();
            }

            return RoutineExpression.From(Enumerator);
        }
    }
}