namespace Hexalith.Domain.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class EventNotSupportedException<TState> : Exception
    {
        public EventNotSupportedException()
        {
        }

        public EventNotSupportedException(string? message)
            : base(message)
        {
        }

        public EventNotSupportedException(string? message, Exception? innerException)
            : base(message, innerException)
        {
        }

        public EventNotSupportedException(Type eventType, string? message = null, Exception? innerException = null)
            : base($"Event '{eventType.FullName}' is not supported by state '{typeof(TState).FullName}'.{message}", innerException)
        {
        }

        protected EventNotSupportedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}