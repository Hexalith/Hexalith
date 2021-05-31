namespace Hexalith.Domain.Messages
{
    using Hexalith.Domain.ValueTypes;

    public abstract class EventBase<TId> : MessageBase<TId>, IEvent
        where TId : BusinessId, new()
    {
        protected EventBase()
        {
        }

        protected EventBase(TId id)
            : base(id)
        {
        }
    }
}