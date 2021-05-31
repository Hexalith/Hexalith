namespace Hexalith.Application.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class EventHandlerNotFoundException : Exception
    {
        public EventHandlerNotFoundException()
        {
        }

        public EventHandlerNotFoundException(string? message) : base(message)
        {
        }

        public EventHandlerNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        public EventHandlerNotFoundException(Type commandType, string? message = null, Exception? innerException = null)
            : base($"The event handler for event '{commandType.Name}' not found. {message}", innerException)
        {
        }

        protected EventHandlerNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}