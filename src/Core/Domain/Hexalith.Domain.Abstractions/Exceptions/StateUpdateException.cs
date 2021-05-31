namespace Hexalith.Domain
{
    using System;
    using System.Runtime.Serialization;
    using System.Text.Json;

    using Hexalith.Domain.Messages;

    [Serializable]
    public class StateUpdateException : Exception
    {
        public StateUpdateException()
        {
        }

        public StateUpdateException(string? message)
            : base(message)
        {
        }

        public StateUpdateException(IEvent? @event, EntityState? state, string? message = null, Exception? innerException = null)
            : base($"Entity state '{state?.GetType()?.Name ?? "??"}' update error with event '{@event?.GetType()?.Name}'.{message}\nEvent:{JsonSerializer.Serialize(@event)}\nState:{JsonSerializer.Serialize(state)}", innerException)
        {
        }

        public StateUpdateException(string? message, Exception? innerException)
            : base(message, innerException)
        {
        }

        protected StateUpdateException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}