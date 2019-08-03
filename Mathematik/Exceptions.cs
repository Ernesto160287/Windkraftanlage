using System;

namespace Mathematik
{
    public class NumericsFailedException : Exception
    {
        public NumericsFailedException()
        {
        }

        public NumericsFailedException(String message) : base(message)
        {
        }

        public NumericsFailedException(String message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class InconsistentInputException : Exception
    {
        public InconsistentInputException()
        {
        }

        public InconsistentInputException(String message) : base(message)
        {
        }

        public InconsistentInputException(String message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class ValueOutOfRangeException : Exception
    {
        public ValueOutOfRangeException()
        {
        }

        public ValueOutOfRangeException(String message) : base(message)
        {
        }

        public ValueOutOfRangeException(String message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
