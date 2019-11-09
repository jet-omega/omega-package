using System;

namespace Omega.Routines.Exceptions
{
    public sealed class RoutineNotCompleteException : AggregateException
    {
        public RoutineNotCompleteException(string message) : base(message)
        {
        }
    }
}