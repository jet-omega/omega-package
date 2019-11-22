using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Omega.Routines
{
    public sealed class GroupRoutine : Routine, IProgressRoutineProvider
    {
        private readonly Routine[] _routines;
        private readonly Stack<IEnumerator>[] _enumeratorsStacks;

        internal GroupRoutine(IEnumerable<Routine> routines)
        {
            _routines = routines.ToArray();
            _enumeratorsStacks = _routines.Select(e => new Stack<IEnumerator>(new[] {e}))
                .ToArray();
        }

        internal GroupRoutine(params Routine[] routines)
            : this((IEnumerable<Routine>) routines)
        {
        }

        protected override IEnumerator RoutineUpdate()
        {
            bool MoveNextAll()
            {
                var flag = false;
                for (int i = 0; i < _enumeratorsStacks.Length; i++)
                {
                    var stack = _enumeratorsStacks[i];
                    if (stack.Count > 0)
                    {
                        var topStack = stack.Peek();
                        if (topStack.MoveNext())
                        {
                            flag = true;
                            var currentTopStackValue = topStack.Current;
                            if (currentTopStackValue is IEnumerator enumerator)
                                stack.Push(enumerator);
                        }
                        else
                        {
                            stack.Pop();
                            i--;
                        }
                    }
                }

                return flag;
            }

            while (MoveNextAll())
                yield return null;
        }

        public GroupRoutine Routines(out Routine[] routines)
        {
            routines = _routines.ToArray();
            return this;
        }

        public float GetProgress()
        {
            var totalCount = _routines.Length;
            var progressPerRoutine = 1f / totalCount;

            var totalProgress = 0f;
            foreach (var routine in _routines)
            {
                if (routine.IsComplete)
                    totalProgress += progressPerRoutine;
                else if (routine is IProgressRoutineProvider progressRoutineProvider)
                {
                    var routineProgress = progressRoutineProvider.GetProgress();
                    totalProgress += routineProgress * progressPerRoutine;
                }
            }

            return totalProgress;
        }
    }
}