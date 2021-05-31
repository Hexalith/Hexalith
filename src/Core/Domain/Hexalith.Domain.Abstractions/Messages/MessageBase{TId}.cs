namespace Hexalith.Domain.Messages
{
    using Hexalith.Domain.ValueTypes;

    public abstract class MessageBase<TId> : IMessage
        where TId : BusinessId, new()
    {
        protected MessageBase()
        {
            Id = new TId();
            MessageId = new MessageId();
        }

        protected MessageBase(TId id)
        {
            Id = id ?? new TId();
            MessageId = new MessageId();
        }

        public string Id { get; init; }

        public string MessageId { get; }
    }
}