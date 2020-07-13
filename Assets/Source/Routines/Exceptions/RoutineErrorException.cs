using System;
using System.Diagnostics;
using JetBrains.Annotations;

namespace Omega.Routines.Exceptions
{
    public class RoutineErrorException : AggregateException
    {
        public RoutineErrorException(string message) : base(message)
        {
        }
    }

    public class NestedRoutineException : AggregateException
    {
        [NotNull] public readonly Routine NestedRoutine;


        internal NestedRoutineException(string message, Routine nestedRoutine)
            :base(message)
        {
            NestedRoutine = nestedRoutine ?? throw new ArgumentNullException(nameof(nestedRoutine));
        }
    }
}