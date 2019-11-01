using System;
using System.Collections;
using Omega.Experimental.Event;

namespace Omega.Routines
{
    public sealed class EnumeratorAsRoutine : Routine
    {
        private readonly IEnumerator _enumerator;

        internal EnumeratorAsRoutine(IEnumerator enumerator)
        {
            _enumerator = enumerator;
        }

        protected override IEnumerator RoutineUpdate()
        {
            while (_enumerator.MoveNext())
                yield return _enumerator.Current;
        }

        public IEnumerator GetEnumerator() => _enumerator;
    }
    
    public sealed class EnumeratorAsRoutineWithController : Routine
    {
        private readonly Func<RoutineControl, IEnumerator> _enumeratorGetter;
        private IEnumerator _enumerator;

        internal EnumeratorAsRoutineWithController(Func<RoutineControl, IEnumerator> enumerator)
        {
            _enumeratorGetter = enumerator;
        }

        protected override IEnumerator RoutineUpdate()
        {
            if (_enumerator == null)
                _enumerator = _enumeratorGetter(new RoutineControl(this));

            while (_enumerator.MoveNext())
                yield return _enumerator.Current;
        }

        public IEnumerator GetEnumerator() => _enumerator;
    }
    
    public sealed class EnumeratorAsRoutineWithResult<T> : Routine<T>
    {
        private readonly Func<RoutineControl<T>, IEnumerator> _enumeratorGetter;
        private IEnumerator _enumerator;

        internal EnumeratorAsRoutineWithResult(Func<RoutineControl<T>, IEnumerator> enumerator)
        {
            _enumeratorGetter = enumerator;
        }

        protected override IEnumerator RoutineUpdate()
        {
            if (_enumerator == null)
                _enumerator = _enumeratorGetter(new RoutineControl<T>(this));

            while (_enumerator.MoveNext())
                yield return _enumerator.Current;
        }

        public IEnumerator GetEnumerator() => _enumerator;
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
}