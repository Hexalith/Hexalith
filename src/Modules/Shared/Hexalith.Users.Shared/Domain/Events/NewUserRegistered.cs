namespace Hexalith.Users.Domain.Events
{
    using Hexalith.Users.Domain.ValueTypes;

    public sealed class NewUserRegistered
        : UserIdEvent
    {
        public NewUserRegistered(UserId UserId, string name) : base(UserId)
            => Name = name;

        public string Name { get; }
    }
}