namespace Hexalith.Infrastructure.EfCore.Helpers
{
    using System;
    using System.Text.Json;

    using Hexalith.Application.Messages;
    using Hexalith.Infrastructure.EfCore.Repositories;

    public static class EnvelopeHelpers
    {
        public static OutboxMessage ToOutboxMessage(this IEnvelope envelope, string sessionId)
        {
            return new OutboxMessage()
            {
                SessionId = sessionId,
                MessageId = envelope.MessageId,
                CausationId = envelope.CausationId?.Value,
                CorrelationId = envelope.CorrelationId?.Value,
                UserName = envelope.UserName.Value,
                SystemUtcDateTime = DateTime.UtcNow,
                EventType = envelope.Message.GetType().AssemblyQualifiedName ?? throw new TypeAccessException($"The type {envelope.Message.GetType().Name} assembly qualified name is null. Can't persist the envelope (MessageId={envelope.MessageId}) in the outbox store."),
                Event = JsonSerializer.Serialize(envelope.Message)
            };
        }
    }
}