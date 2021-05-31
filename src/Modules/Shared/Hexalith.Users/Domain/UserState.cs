namespace Bistrotic.Users.Domain
{
    using System;

    using Bistrotic.Domain;
    using Bistrotic.Domain.Messages;
    using Bistrotic.Users.Domain.Errors;
    using Bistrotic.Users.Domain.Events;

    public sealed class Userstate : EntityState
    {
        public DateTime? BirthDate { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Name { get; set; }

        public void Apply(IEvent @event)
        {
            switch (@event)
            {
                case NewUserRegistered e:
                    Apply(e);
                    break;

                case RenamedUser e:
                    Apply(e);
                    break;

                case UserRenameAttemptFailed e:
                    Apply(e);
                    break;

                case UserIdentityChanged e:
                    Apply(e);
                    break;

                default:
                    throw new NotSupportedException($"The event '{@event.GetType().Name}' not supported by '{nameof(Userstate)}'.");
            };
        }

        public void Apply(NewUserRegistered @event)
        {
            Name = @event.Name;
        }

        public void Apply(RenamedUser @event)
        {
            Name = @event.NewName;
        }

#pragma warning disable CA1822 // Mark members as static

        public void Apply(UserRenameAttemptFailed _)
#pragma warning restore CA1822 // Mark members as static
        {
        }

        public void Apply(UserIdentityChanged @event)
        {
            FirstName = @event.NewFirstName;
            LastName = @event.NewLastName;
            BirthDate = @event.NewBirthDate;
        }
    }
}