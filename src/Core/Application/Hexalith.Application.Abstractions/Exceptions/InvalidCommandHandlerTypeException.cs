namespace Hexalith.Application.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class InvalidCommandHandlerTypeException : Exception
    {
        public InvalidCommandHandlerTypeException()
        {
        }

        public InvalidCommandHandlerTypeException(string? message) : base(message)
        {
        }

        public InvalidCommandHandlerTypeException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        public InvalidCommandHandlerTypeException(Type handlerType, Type expectedType, string? message = null, Exception? innerException = null)
            : base($"The Command handler '{handlerType.Name}' should have a type of '{expectedType.Name}'. {message}", innerException)
        {
        }

        protected InvalidCommandHandlerTypeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}