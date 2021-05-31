using Hexalith.Domain.Messages;
using Hexalith.Users.Domain.Events;
using Hexalith.Users.Domain.ValueTypes;

namespace Hexalith.Users.Domain.Errors
{
    public class UserRenameAttemptFailed : UserIdEvent, IErrorEvent
    {
        public UserRenameAttemptFailed(UserId UserId, string name, string expectedName, string newName)
            : base(UserId) => (Name, ExpectedName, NewName) = (name, expectedName, newName);

        public string ErrorMessage => $"The user name can't be changed to '{NewName}'. The name has been modified by another user. Expected name : '{ExpectedName}'; Actual name : '{Name}'.";
        public string ExpectedName { get; }
        public string Name { get; }
        public string NewName { get; }
    }
}