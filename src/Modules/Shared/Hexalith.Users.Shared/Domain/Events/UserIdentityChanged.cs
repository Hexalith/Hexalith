namespace Hexalith.Users.Domain.Events
{
    using System;

    using Hexalith.Users.Domain.ValueTypes;

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
             => (OldFirstName, OldLastName, OldBirthDate, NewFirstName, NewLastName, NewBirthDate) =
               (oldFirstName, oldLastName, oldBirthDate, newFirstName, newLastName, newBirthDate);

        public DateTime? NewBirthDate { get; init; }
        public string? NewFirstName { get; init; }
        public string? NewLastName { get; init; }
        public DateTime? OldBirthDate { get; init; }
        public string? OldFirstName { get; init; }
        public string? OldLastName { get; init; }
    }
}