namespace Hexalith.Application.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class InvalidEventHandlerTypeException : Exception
    {
        public InvalidEventHandlerTypeException()
        {
        }

        public InvalidEventHandlerTypeException(string? message) : base(message)
        {
        }

        public InvalidEventHandlerTypeException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        public InvalidEventHandlerTypeException(Type handlerType, Type expectedType, string? message = null, Exception? innerException = null)
            : base($"The Event handler '{handlerType.Name}' should have a type of '{expectedType.Name}'. {message}", innerException)
        {
        }

        protected InvalidEventHandlerTypeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}