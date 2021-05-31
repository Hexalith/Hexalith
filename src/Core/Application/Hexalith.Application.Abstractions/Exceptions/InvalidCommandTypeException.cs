namespace Hexalith.Application.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class InvalidCommandTypeException : Exception
    {
        public InvalidCommandTypeException() : base()
        {
        }

        public InvalidCommandTypeException(string? message) : base(message)
        {
        }

        public InvalidCommandTypeException(Type CommandType, string? message = null, Exception? innerException = null)
            : base($"The Command type '{CommandType.FullName}' is invalid.\n{message}", innerException)
        {
        }

        public InvalidCommandTypeException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidCommandTypeException(SerializationInfo serializationInfo, StreamingContext streamingContext)
                    : base(serializationInfo, streamingContext)
        {
        }
    }
}