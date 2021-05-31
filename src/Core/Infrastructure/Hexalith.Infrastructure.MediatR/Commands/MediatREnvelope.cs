namespace Hexalith.Infrastructure.MediatR.Queries
{
    using System;

    using Hexalith.Application.Messages;
    using Hexalith.Domain.Messages;
    using Hexalith.Domain.ValueTypes;

    using global::MediatR;

    public sealed class MediatREnvelope<TMessage> : Envelope<TMessage>, INotification, IEquatable<MediatREnvelope<TMessage>>
        where TMessage : class
    {
        public MediatREnvelope(TMessage message, MessageId messageId, UserName userName, DateTimeOffset userDateTime, MessageId? correlationId = null)
            : base(message, messageId, userName, userDateTime, correlationId)
        {
        }

        public MediatREnvelope(TMessage message, MessageId messageId, IEnvelope parent) : base(message, messageId, parent)
        {
        }

        public MediatREnvelope(IEnvelope envelope) : base(envelope)
        {
        }

        public bool Equals(MediatREnvelope<TMessage>? other)
            => other != null && other.MessageId == MessageId;

        public override bool Equals(object? obj)
        {
            return Equals(obj as MediatREnvelope<TMessage>);
        }

        public override int GetHashCode() => Message.GetHashCode();
    }
}