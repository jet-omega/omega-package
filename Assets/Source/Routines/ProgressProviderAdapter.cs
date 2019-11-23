namespace Omega.Routines
{
    internal sealed class ProgressProviderAdapter : IProgressRoutineProvider
    {
        private Routine _routine;

        public ProgressProviderAdapter(Routine routine)
        {
            _routine = routine;
        }

        public float GetProgress()
        {
            return _routine.IsComplete ? 1 : 0;
        }
    }
}