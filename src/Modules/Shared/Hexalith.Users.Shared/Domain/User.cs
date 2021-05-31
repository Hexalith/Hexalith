namespace Hexalith.Users.Domain
{
    using System;
    using System.Collections.Generic;

    using Hexalith.Domain;
    using Hexalith.Domain.Messages;
    using Hexalith.Users.Domain.Errors;
    using Hexalith.Users.Domain.Events;
    using Hexalith.Users.Domain.ValueTypes;

    public class User : IAggregateRoot
    {
        private readonly UserId _userId;

        public User(UserId userId, Userstate state)
        => (_userId, State) = (
            userId ?? throw new ArgumentNullException(nameof(userId)),
            state ?? throw new ArgumentNullException(nameof(state))
            );

        public string AggregateId => _userId.Value;

        public string AggregateName => nameof(User);

        public Userstate State { get; }

        public static IEnumerable<IEvent> RegisterNewUser(
            UserId userId,
            string name,
            out User User)
        {
            User = new User(userId, new Userstate() { Name = name });
            var e = new NewUserRegistered(userId, name);
            User.State.Apply(e);
            return new IEvent[] { e };
        }

        public IEnumerable<IEvent> ChangeUserIdentity(
            string? oldFirstName,
            string? oldLastName,
            DateTime? oldBirthDate,
            string? newFirstName,
            string? newLastName,
            DateTime? newBirthDate
            )
        {
            // Concurrency Check
            if (State.FirstName != oldFirstName ||
                State.LastName != oldLastName ||
                State.BirthDate != oldBirthDate
                )
            {
                return new IEvent[] { new ChangeUserIdentityAttemptFailed(
                    AggregateId,
                    State.FirstName,
                    State.LastName,
                    State.BirthDate,
                    oldFirstName,
                    oldLastName,
                    oldBirthDate,
                    newFirstName,
                    newLastName,
                    newBirthDate
                    )};
            }
            return new IEvent[] { new UserIdentityChanged(
                    AggregateId,
                    oldFirstName,
                    oldLastName,
                    oldBirthDate,
                    newFirstName,
                    newLastName,
                    newBirthDate
                    ) };
        }

        public IEnumerable<IEvent> RenameUser(string oldName, string newName)
        {
            // Concurrency Check
            if (State.Name != oldName)
            {
                return new IEvent[] { new UserRenameAttemptFailed(
                    AggregateId,
                    State.Name ?? string.Empty,
                    oldName,
                    newName
                    )};
            }
            return new IEvent[] { new RenamedUser(
                    AggregateId,
                    oldName,
                    newName
                    ) };
        }
    }
}