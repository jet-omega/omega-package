using System;
using Omega.Package;

namespace Omega.Routines
{
    public struct RoutineControl<T>
    {
        private readonly ByEnumeratorRoutine<T> _routine;

        internal RoutineControl(ByEnumeratorRoutine<T> routine)
        {
            _routine = routine;
        }

        public Routine<T> GetRoutine() => _routine;

        public void SetProgress(float progress)
        {
            _routine.SetProgress(progress);
        }
        
        public void SetResult(T result)
        {
            if (_routine == null)
                throw ExceptionHelper.SetResultCannotCalledWhenRoutineIsNotDefined;
            
            _routine.SetResultInternal(result);
        }

        public void OnCancellationCallback(Action action)
        {
            _routine.OnCancelation = action;
        }

        public void OnForcedCallback(Action action)
        {
            _routine.OnForceComplete = action;
        }
        
    }

    public struct RoutineControl
    {
        private readonly ByEnumeratorRoutine _routine;

        internal RoutineControl(ByEnumeratorRoutine routine)
        {
            _routine = routine;
        }
        public Routine GetRoutine() => _routine;

        public void SetProgress(float value)
        {
            _routine.SetProgress(value);
        }
        
        public void OnCancellationCallback(Action action)
        {
            _routine.OnCancelation = action;
        }

        public void OnForcedCallback(Action action)
        {
            _routine.OnForceComplete = action;
        }
    }
}