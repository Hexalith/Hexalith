namespace Hexalith.Emails.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Hexalith.Emails.Contracts.Events;
    using Hexalith.Emails.Contracts.ValueTypes;
    using Hexalith.Emails.Domain.Exceptions;
    using Hexalith.Emails.Domain.States;

    public class Email
    {
        private readonly string _id;
        private readonly IEmailState _state;

        public Email(string id, IEmailState state)
        {
            _state = state ?? throw new EmailStateNotInitializedException("Email Id: " + _id);
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new UndefinedEmailIdException();
            }
            _id = id;
        }

        public Task<IEnumerable<object>> Receive(
            string recipient,
            string subject,
            string body,
            string sender,
            IEnumerable<string> toRecipients,
            IEnumerable<string> copyToRecipients,
            IEnumerable<Attachment> attachments
            )
        {
            List<object> events = new()
            {
                new EmailReceived
                {
                    Recipient = recipient,
                    EmailId = _id,
                    Subject = subject,
                    Body = body,
                    Sender = sender,
                    ToRecipients = toRecipients,
                    CopyToRecipients = copyToRecipients,
                    Attachments = attachments
                }
            };
            return Apply(events);
        }

        private Task<IEnumerable<object>> Apply(IEnumerable<object> events)
        {
            foreach (var e in events)
            {
                switch (e)
                {
                    case EmailReceived @event:
                        _state.Apply(@event);
                        break;

                    default:
                        throw new NotSupportedException($"The event type '{e.GetType().Name}' is not supported by '{_state.GetType().Name}'.");
                }
            }
            return Task.FromResult(events);
        }
    }
}