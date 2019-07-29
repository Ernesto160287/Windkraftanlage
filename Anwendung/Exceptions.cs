using System;

namespace Anwendung
{
    class CharacteristicCurveInputException : Exception
    {
        public CharacteristicCurveInputException()
        {
        }

        public CharacteristicCurveInputException(String message) : base(message)
        {
        }

        public CharacteristicCurveInputException(String message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
