using System;

namespace Omega.Routines
{
    public struct RoutineProgressHandler
    {
        private IProgressRoutineProvider _routine;
        
        public float Progress => _routine.GetProgress();
        private float _previousProgress;

        public bool TryUpdateProgress(out float progress)
        {
            if(_routine == null)
                throw new InvalidOperationException();
            
            progress = Progress;
            if (Math.Abs(_previousProgress - progress) < 0.00001f)
                return false;
            
            _previousProgress = progress;
            return true;
        }

        internal RoutineProgressHandler(IProgressRoutineProvider routine)
        {
            _routine = routine;
            _previousProgress = -1;
        }

        public RoutineProgressHandler(Routine routine)
        {
            _routine = RoutineUtilities.GetProgressRoutineProvider(routine);
            _previousProgress = _routine.GetProgress();
        }
    }
}