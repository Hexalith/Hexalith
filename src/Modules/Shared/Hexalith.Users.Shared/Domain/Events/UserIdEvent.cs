namespace Hexalith.Users.Domain.Events
{
    using Hexalith.Domain.Messages;
    using Hexalith.Users.Domain.ValueTypes;

    public abstract class UserIdEvent : EventBase<UserId>
    {
        protected UserIdEvent(UserId UserId)
            : base(UserId) { }
    }
}