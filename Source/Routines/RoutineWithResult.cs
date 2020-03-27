using Omega.Routines.Exceptions;

namespace Omega.Routines
{
    public abstract class Routine<TResult> : Routine
    {
        private TResult _result;

        internal void SetResultInternal(TResult result)
        {
            _result = result;
        }

        protected void SetResult(TResult result)
        {
            _result = result;
        }

        public TResult GetResult()
        {
            if (IsError)
                throw new RoutineErrorException(
                    "It is impossible to get the result of the routine, because the routine contains an error." +
                    " Use the IsError property to determine if the routine contains an error.");

            if (!IsComplete)
                throw new RoutineNotCompleteException(
                    "It is not possible to get a routine result because the routine has not yet been completed." +
                    " Use the IsComplete property to determine the completion of a routine");

            return _result;
        }

        public TResult WaitResult()
        {
            RoutineUtilities.CompleteWithoutChecks(this);

            if (IsError)
                throw new RoutineErrorException(
                    "It is impossible to get the result of the routine, because the routine contains an error");

            return _result;
        }

        public struct ResultContainer
        {
            private readonly Routine<TResult> _routine;
            public Routine<TResult> Routine => _routine;


            public TResult Result
            {
                get
                {
                    if (_routine.IsError)
                        throw new RoutineErrorException(
                            "It is impossible to get the result, because the routine contains an error." +
                            $" Use the IsError property in {nameof(Routine)} to determine if the routine contains an error.");

                    if (!_routine.IsComplete)
                        throw new RoutineNotCompleteException(
                            "It is not possible to get a result because the routine has not yet been completed." +
                            $" Use the IsComplete property in {nameof(Routine)} to determine the completion of a routine");

                    return _routine._result;
                }
            }

            public ResultContainer(Routine<TResult> routine)
            {
                _routine = routine;
            }

            public static implicit operator TResult(ResultContainer resultContainer)
            {
                return resultContainer.Result;
            }
            
        }
    }
}