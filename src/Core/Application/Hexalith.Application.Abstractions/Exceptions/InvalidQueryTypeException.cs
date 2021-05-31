namespace Hexalith.Application.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class InvalidQueryTypeException : Exception
    {
        public InvalidQueryTypeException() : base()
        {
        }

        public InvalidQueryTypeException(string? message) : base(message)
        {
        }

        public InvalidQueryTypeException(Type queryType, string? message = null, Exception? innerException = null)
            : base($"The query type '{queryType.FullName}' is invalid.\n{message}", innerException)
        {
        }

        public InvalidQueryTypeException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidQueryTypeException(SerializationInfo serializationInfo, StreamingContext streamingContext)
                    : base(serializationInfo, streamingContext)
        {
        }
    }
}