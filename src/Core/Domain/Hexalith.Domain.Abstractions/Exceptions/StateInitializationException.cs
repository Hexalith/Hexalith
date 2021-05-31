namespace Hexalith.Domain
{
    using System;
    using System.Runtime.Serialization;
    using System.Text.Json;

    using Hexalith.Domain.Messages;

    [Serializable]
    public class StateInitializationException : Exception
    {
        public StateInitializationException()
            : this(null, null, null)
        {
        }

        public StateInitializationException(string? message)
            : this(null, null, message)
        {
        }

        public StateInitializationException(IEvent? @event, EntityState? state, string? message = null, Exception? innerException = null)
            : base($"Entity state '{state?.GetType()?.Name ?? "??"}' initialisation error with event '{@event?.GetType()?.Name ?? "??"}'.{message}\nEvent:{JsonSerializer.Serialize(@event)}\nState:{JsonSerializer.Serialize(state)}", innerException)
        {
        }

        public StateInitializationException(string? message, Exception? innerException)
            : this(null, null, message, innerException)
        {
        }

        protected StateInitializationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}