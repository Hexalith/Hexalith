namespace Bistrotic.Users.Application.Domain.Commands
{
    using Bistrotic.Infrastructure.CodeGeneration.Attributes;
    using Bistrotic.Users.Application.Commands;
    using Bistrotic.Users.Domain.ValueTypes;

    [ApiCommand]
    public sealed class RenameUser :
        UserIdCommand
    {
        public RenameUser(UserId UserId, string newName) : base(UserId)
        {
            NewName = newName;
        }

        public string NewName { get; }
    }
}