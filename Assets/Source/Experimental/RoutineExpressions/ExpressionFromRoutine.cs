using System;

namespace Omega.Routines.Experimental
{
    internal sealed class ExpressionFromRoutine<T> : IRoutineExpression<T>
    {
        private readonly Func<Routine<T>> _routineFactory;

        public ExpressionFromRoutine(Func<Routine<T>> routineFactory) 
            => _routineFactory = routineFactory;

        public Routine<T> ToRoutine()
            => _routineFactory.Invoke();

        Routine IRoutineExpression.ToRoutine()
            => ToRoutine();
    }

    internal sealed class ExpressionFromRoutine : IRoutineExpression
    {
        private readonly Func<Routine> _routineFactory;

        public ExpressionFromRoutine(Func<Routine> routineFactory)
        {
            _routineFactory = routineFactory;
        }

        public Routine ToRoutine()
            => _routineFactory.Invoke();
    }
}