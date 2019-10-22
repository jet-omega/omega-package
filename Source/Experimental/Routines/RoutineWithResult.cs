using System;

namespace Omega.Experimental.Routines
{
    public abstract class Routine<TResult> : Routine
    {
        private TResult _result;

        public Routine<TResult> Result(out ResultContainer result)
        {
            result = new ResultContainer(this);
            return this;
        }

        protected sealed override void SetComplete()
        {
            throw new NotSupportedException();
        }

        protected void SetComplete(TResult result)
        {
            _result = result;
            base.SetComplete();
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