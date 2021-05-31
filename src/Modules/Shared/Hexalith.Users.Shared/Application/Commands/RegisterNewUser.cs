namespace Hexalith.Users.Application.Commands
{
    using Hexalith.Application.Commands;
    using Hexalith.Domain.Contracts.Commands;
    using Hexalith.Users.Domain.ValueTypes;

    [Command]
    public sealed class RegisterNewUser
    {
        public RegisterNewUser() => UserId = Name = string.Empty;

        public RegisterNewUser(UserId userId, string name)
            => (Name, UserId) = (name, userId);

        public string Name { get; init; }
        public string UserId { get; init; }
    }
}