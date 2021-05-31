namespace Hexalith.Users.Application.Commands
{
    using System;

    using Hexalith.Application.Commands;
    using Hexalith.Domain.Contracts.Commands;
    using Hexalith.Users.Domain.ValueTypes;

    [Command]
    public sealed record ChangeUserIdentity
    {
        public ChangeUserIdentity()
        {
            UserId = string.Empty;
        }
        public ChangeUserIdentity(
            UserId userId,
            string? oldFirstName = null,
            string? oldLastName = null,
            DateTime? oldBirthDate = null,
            string? newFirstName = null,
            string? newLastName = null,
            DateTime? newBirthDate = null
            )
            => (UserId, OldFirstName, OldLastName, OldBirthDate, NewFirstName, NewLastName, NewBirthDate) =
               (userId, oldFirstName, oldLastName, oldBirthDate, newFirstName, newLastName, newBirthDate);

        public string UserId { get; init; }
        public string? OldFirstName { get; init; }
        public string? OldLastName { get; init; }
        public DateTime? OldBirthDate { get; init; }
        public string? NewFirstName { get; init; }
        public string? NewLastName { get; init; }
        public DateTime? NewBirthDate { get; init; }
    }
}