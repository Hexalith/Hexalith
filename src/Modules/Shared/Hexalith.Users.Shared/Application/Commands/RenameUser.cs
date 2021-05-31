namespace Hexalith.Users.Application.Domain.Commands
{
    using Hexalith.Application.Commands;
    using Hexalith.Domain.Contracts.Commands;
    using Hexalith.Users.Domain.ValueTypes;

    [Command]
    public sealed class RenameUser
    {
        public RenameUser() => UserId = OldName = NewName = string.Empty;

        public RenameUser(UserId userId, string oldName, string newName)
            => (UserId, OldName, NewName) = (userId, oldName, newName);

        public string OldName { get; init; }
        public string NewName { get; init; }
        public string UserId { get; init; }
    }
}