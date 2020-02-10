using System;

namespace Omega.Routines.Experimental
{
    internal class ExpressionFromRoutine<T> : IRoutineExpression<T>
    {
        private Func<Routine<T>> _routineFactory;

        public ExpressionFromRoutine(Func<Routine<T>> routine) =>
            _routineFactory = routine;

        public Routine<T> ToRoutine()
            => _routineFactory.Invoke();

        Routine IRoutineExpression.ToRoutine()
            => ToRoutine();
    }

    internal class ExpressionFromRoutine : IRoutineExpression
    {
        private Func<Routine> _routineFactory;

        public ExpressionFromRoutine(Func<Routine> routine)
            => _routineFactory = routine;

        public Routine ToRoutine()
            => _routineFactory.Invoke();
    }
}