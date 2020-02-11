using System;
using System.Collections;
using System.Threading;

namespace Omega.Routines.Experimental
{
    public static class RoutineExpression
    {
        public static IRoutineExpression Order(IRoutineExpression before, IRoutineExpression after)
        {
            return Order(new[] {before, after});
        }

        public static IRoutineExpression Order(params IRoutineExpression[] expressions)
        {
            return new OrderExpression(expressions);
        }

        public static IRoutineExpression Group(params IRoutineExpression[] expressions)
        {
            return new GroupExpression(expressions);
        }

        public static IRoutineExpression<T> Task<T>(Func<T> task)
        {
            return FromRoutine(() => Routine.Task(task));
        }

        public static IRoutineExpression Task(Action task)
        {
            return FromRoutine(() => Routine.Task(task));
        }

        public static IRoutineExpression<T> Task<T>(Func<CancellationToken, T> task)
        {
            return FromRoutine(() => Routine.Task(task));
        }

        public static IRoutineExpression Task(Action<CancellationToken> task)
        {
            return FromRoutine(() => Routine.Task(task));
        }

        public static IRoutineExpression Action(Action action)
        {
            return FromRoutine(() => Routine.ByAction(action));
        }

        public static IRoutineExpression<T> Action<T>(Func<T> action)
        {
            return FromRoutine(() => Routine.ByAction(action));
        }

        public static IRoutineExpression FromRoutine(Func<Routine> routine)
        {
            return new ExpressionFromRoutine(routine);
        }

        public static IRoutineExpression<T> FromRoutine<T>(Func<Routine<T>> routine)
        {
            return new ExpressionFromRoutine<T>(routine);
        }

        internal static IRoutineExpression From(Func<RoutineControl, IEnumerator> enumeratorFactory)
        {
            return FromRoutine(() => Routine.ByEnumerator(enumeratorFactory));
        }

        internal static IRoutineExpression New()
        {
            return EmptyExpression.Instance.Value;
        }

        internal static IRoutineExpression<T> From<T>(Func<RoutineControl<T>, IEnumerator> enumeratorFactory)
        {
            return FromRoutine(() => Routine.ByEnumerator(enumeratorFactory));
        }
    }

    internal sealed class EmptyExpression : IRoutineExpression
    {
        public readonly static Lazy<IRoutineExpression> Instance
            = new Lazy<IRoutineExpression>(() => new EmptyExpression());

        public Routine ToRoutine() => Routine.FromCompleted();
    }
}