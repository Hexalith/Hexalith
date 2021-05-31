namespace Hexalith.Application.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class InvalidQueryHandlerTypeException : Exception
    {
        public InvalidQueryHandlerTypeException()
        {
        }

        public InvalidQueryHandlerTypeException(string? message) : base(message)
        {
        }

        public InvalidQueryHandlerTypeException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        public InvalidQueryHandlerTypeException(Type handlerType, Type expectedType, string? message = null, Exception? innerException = null)
            : base($"The query handler '{handlerType.Name}' should have a type of '{expectedType.Name}'. {message}", innerException)
        {
        }

        protected InvalidQueryHandlerTypeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}