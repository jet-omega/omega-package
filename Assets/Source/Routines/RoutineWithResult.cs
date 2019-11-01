using System;

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
                throw new Exception();
            
            if(!IsComplete)
                throw new Exception();

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
                        throw new Exception();
                    
                    if(!_routine.IsComplete)
                        throw new Exception();

                    return _routine._result;
                }
            }

            public ResultContainer(Routine<TResult> routine)
            {
                _routine = routine;
            }
        }
    }
}