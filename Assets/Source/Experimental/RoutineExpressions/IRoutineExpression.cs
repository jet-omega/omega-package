namespace Omega.Routines.Experimental
{
    public interface IRoutineExpression
    {
        Routine ToRoutine();
    }

    public interface IRoutineExpression<T> : IRoutineExpression
    {
        new Routine<T> ToRoutine();
    }
}