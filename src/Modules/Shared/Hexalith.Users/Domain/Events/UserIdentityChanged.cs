namespace Bistrotic.Users.Domain.Events
{
    using System;

    using Bistrotic.Users.Domain.ValueTypes;

    public sealed class UserIdentityChanged :
        UserIdEvent
    {
        public UserIdentityChanged(
            UserId UserId,
            string? oldFirstName,
            string? oldLastName,
            DateTime? oldBirthDate,
            string? newFirstName,
            string? newLastName,
            DateTime? newBirthDate
            ) : base(UserId)
        {
            NewFirstName = newFirstName;
            NewLastName = newLastName;
            NewBirthDate = newBirthDate;
            OldFirstName = oldFirstName;
            OldLastName = oldLastName;
            OldBirthDate = oldBirthDate;
        }

        public DateTime? NewBirthDate { get; init; }
        public string? NewFirstName { get; init; }
        public string? NewLastName { get; init; }
        public DateTime? OldBirthDate { get; init; }
        public string? OldFirstName { get; init; }
        public string? OldLastName { get; init; }
    }
}