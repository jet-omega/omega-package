using Omega.Package;

namespace Omega.Routines
{
    public struct RoutineControl<T>
    {
        private Routine<T> _routine;

        internal RoutineControl(Routine<T> routine)
        {
            _routine = routine;
        }

        public Routine<T> GetRoutine() => _routine;

        public void SetResult(T result)
        {
            if (_routine == null)
                throw ExceptionHelper.SetResultCannotCalledWhenRoutineIsNotDefined;
            
            _routine.SetResultInternal(result);
        }
    }

    public struct RoutineControl
    {
        private Routine _routine;

        internal RoutineControl(Routine routine)
        {
            _routine = routine;
        }

        public Routine GetRoutine() => _routine;
    }
}