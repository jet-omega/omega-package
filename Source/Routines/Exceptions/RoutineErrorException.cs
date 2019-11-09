using System;

namespace Omega.Routines.Exceptions
{
    public class RoutineErrorException : AggregateException
    {
        public RoutineErrorException(string message) : base(message)
        {
        }
    }
}