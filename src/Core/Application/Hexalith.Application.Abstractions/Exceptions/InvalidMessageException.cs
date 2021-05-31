namespace Hexalith.Application.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class InvalidMessageException : Exception
    {
        public InvalidMessageException()
        {
        }

        public InvalidMessageException(string? message) : base(message)
        {
        }

        public InvalidMessageException(Type messageType, string jsonValue, string? message = null, Exception? innerException = null)
            : this($"The message type '{messageType.FullName}' is not a message type or can't be desialized from value : \n{jsonValue}.\n{message}", innerException)
        {
        }

        public InvalidMessageException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidMessageException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}