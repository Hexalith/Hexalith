namespace Bistrotic.Users.Domain
{
    using System;
    using System.Collections.Generic;

    using Bistrotic.Domain;
    using Bistrotic.Domain.Messages;
    using Bistrotic.Users.Domain.Errors;
    using Bistrotic.Users.Domain.Events;
    using Bistrotic.Users.Domain.ValueTypes;

    public class User : IEntity, IAggregateRoot
    {
        private readonly UserId _UserId;

        public User(UserId UserId, Userstate state)
        {
            State = state ?? throw new ArgumentNullException(nameof(state));
            _UserId = UserId ?? throw new ArgumentNullException(nameof(UserId));
        }

        public string AggregateId => _UserId.Value;

        public string AggregateName => nameof(User);

        public Userstate State { get; }

        public static IEnumerable<IEvent> RegisterNewUser(
            UserId UserId,
            string name,
            out User User)
        {
            User = new User(UserId, new Userstate() { Name = name });
            var e = new NewUserRegistered(UserId, name);
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