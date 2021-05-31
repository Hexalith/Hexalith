namespace Bistrotic.Users.Domain.Events
{
    using Bistrotic.Users.Domain.ValueTypes;

    public sealed class NewUserRegistered
        : UserIdEvent
    {
        public NewUserRegistered(UserId UserId, string name) : base(UserId)
        {
            Name = name;
        }

        public string Name { get; }
    }
}