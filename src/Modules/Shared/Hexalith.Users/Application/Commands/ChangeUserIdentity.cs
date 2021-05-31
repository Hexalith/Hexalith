namespace Bistrotic.Users.Application.Domain.Commands
{
    using System;

    using Bistrotic.Infrastructure.CodeGeneration.Attributes;
    using Bistrotic.Users.Application.Commands;
    using Bistrotic.Users.Domain.ValueTypes;
    [ApiCommand]
    public sealed class ChangeUserIdentity : UserIdCommand
    {
        public ChangeUserIdentity(UserId UserId, string? firstName = null, string? lastName = null, DateTime? birthDate = null) : base(UserId)
        {
            FirstName = firstName ?? string.Empty;
            LastName = lastName ?? string.Empty;
            BirthDate = birthDate ?? DateTime.MinValue;
        }

        public DateTime BirthDate { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
    }
}