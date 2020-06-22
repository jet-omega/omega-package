using System;
using System.Collections;

namespace Omega.Routines.Tests
{
    internal class EmptyTestRoutine : Routine
    {
        public SkipMode Mode;

        public EmptyTestRoutine(SkipMode mode)
        {
            Mode = mode;
        }

        protected override IEnumerator RoutineUpdate()
        {
            switch (Mode)
            {
                case SkipMode.NoEnumerator:
                    return null;
                case SkipMode.EmptyEnumerator:
                    return Empty();
                case SkipMode.OnceSkipEnumerator:
                    return Once();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private new static IEnumerator Empty()
        {
            yield break;
        }

        private static IEnumerator Once()
        {
            yield return null;
        }

        public enum SkipMode
        {
            NoEnumerator,
            EmptyEnumerator,
            OnceSkipEnumerator
        }
    }
}