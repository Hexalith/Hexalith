namespace Bistrotic.Users.Domain
{
    using System;

    using Bistrotic.Domain.Messages;
    using Bistrotic.Users.Domain.Events;
    using Bistrotic.Users.Domain.ValueTypes;

    public class ChangeUserIdentityAttemptFailed : UserIdEvent, IErrorEvent
    {
        public ChangeUserIdentityAttemptFailed(
            UserId UserId,
            string? firstName,
            string? lastName,
            DateTime? birthDate,
            string? expectedFirstName,
            string? expectedLastName,
            DateTime? expectedBirthDate,
            string? newFirstName,
            string? newLastName,
            DateTime? newBirthDate
            )
            : base(UserId)
        {
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            ExpectedFirstName = expectedFirstName;
            ExpectedLastName = expectedLastName;
            ExpectedBirthDate = expectedBirthDate;
            NewFirstName = newFirstName;
            NewLastName = newLastName;
            NewBirthDate = newBirthDate;
        }

        public DateTime? BirthDate { get; }

        public string ErrorMessage
        {
            get
            {
                var message = $"The user '{Id}' identity cannot be changed to : First name = '{NewFirstName}'; Last name = '{NewLastName}'; Birth date = '{NewBirthDate}';\nThe identity has been changed by another user.";
                if (FirstName != ExpectedFirstName)
                {
                    message += $"\nExpected value for first name is '{ExpectedFirstName}' but the actual value in the system is '{FirstName}'.";
                }
                if (LastName != ExpectedLastName)
                {
                    message += $"\nExpected value for last name is '{ExpectedLastName}' but the actual value in the system is '{LastName}'.";
                }
                if (FirstName != ExpectedFirstName)
                {
                    message += $"\nExpected value for birth date is '{ExpectedBirthDate}' but the actual value in the system is '{BirthDate}'.";
                }
                return message;
            }
        }

        public DateTime? ExpectedBirthDate { get; init; }
        public string? ExpectedFirstName { get; init; }
        public string? ExpectedLastName { get; init; }
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public DateTime? NewBirthDate { get; init; }
        public string? NewFirstName { get; init; }
        public string? NewLastName { get; init; }
    }
}