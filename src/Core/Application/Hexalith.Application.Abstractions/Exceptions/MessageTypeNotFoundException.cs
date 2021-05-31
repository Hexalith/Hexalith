namespace Hexalith.Application.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class MessageTypeNotFoundException : Exception
    {
        public MessageTypeNotFoundException() : this(null, null, null)
        {
        }

        public MessageTypeNotFoundException(string? messageType) : this(messageType, null, null)
        {
        }

        public MessageTypeNotFoundException(string? messageType, string? message) : this(messageType, message, null)
        {
        }

        public MessageTypeNotFoundException(string? messageType, string? message, Exception? innerException) :
            base($"The message type with name '{messageType}' does not exist.{message}.", innerException)
        {
        }

        public MessageTypeNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected MessageTypeNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}