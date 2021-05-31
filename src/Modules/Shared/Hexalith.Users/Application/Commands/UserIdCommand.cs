namespace Bistrotic.Users.Application.Commands
{
    using Bistrotic.Application.Commands;
    using Bistrotic.Users.Domain.ValueTypes;

    public abstract class UserIdCommand : Command<UserId>
    {
        protected UserIdCommand(UserId UserId)
            : base(UserId)
        {
        }
    }
}