using System;

using Hexalith.Domain.ValueTypes;

namespace Hexalith.Application.Messages
{
    public class Envelope : IEnvelope
    {
        public Envelope(object message, MessageId messageId, UserName userName, DateTimeOffset userDateTime, MessageId? correlationId = null, MessageId? causationId = null)
        {
            UserName = userName;
            Message = message;
            CorrelationId = correlationId;
            CausationId = causationId;
            MessageId = messageId;
            UserDateTime = userDateTime;
        }

        public Envelope(object message, MessageId messageId, IEnvelope parent)
        {
            UserDateTime = parent.UserDateTime;
            UserName = parent.UserName;
            Message = message;
            CorrelationId = parent.CorrelationId ?? parent.MessageId;
            CausationId = parent.MessageId;
            MessageId = messageId;
        }

        public Envelope(IEnvelope envelope)
        {
            UserName = envelope.UserName;
            Message = envelope.Message;
            CorrelationId = envelope.CorrelationId;
            CausationId = envelope.CausationId;
            MessageId = envelope.MessageId;
            UserDateTime = envelope.UserDateTime;
        }

        public MessageId? CausationId { get; init; }
        public MessageId? CorrelationId { get; init; }
        public object Message { get; init; }
        public MessageId MessageId { get; init; }
        public DateTimeOffset UserDateTime { get; init; }
        public UserName UserName { get; init; }
        object IEnvelope.Message => Message;
    }

    public class Envelope<T> : Envelope
        where T : class
    {
        public Envelope(IEnvelope envelope) : base(envelope)
        {
        }

        public Envelope(T message, MessageId messageId, IEnvelope parent) : base(message, messageId, parent)
        {
        }

        public Envelope(T message, MessageId messageId, UserName userName, DateTimeOffset userDateTime, MessageId? correlationId = null, MessageId? causationId = null) : base(message, messageId, userName, userDateTime, correlationId, causationId)
        {
        }

        public new T Message => (T)base.Message;
    }
}