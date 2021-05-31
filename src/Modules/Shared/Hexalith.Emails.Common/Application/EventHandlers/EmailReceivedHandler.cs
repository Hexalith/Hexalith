namespace Hexalith.Emails.Application.EventHandlers
{
    using System.Threading;
    using System.Threading.Tasks;

    using Hexalith.Application.Events;
    using Hexalith.Application.Messages;
    using Hexalith.Emails.Application.Services;
    using Hexalith.Emails.Contracts.Events;

    [EventHandler(Event = typeof(EmailReceived))]
    public class EmailReceivedHandler : IEventHandler<EmailReceived>
    {
        private readonly IMailboxService _mailService;

        public EmailReceivedHandler(IMailboxService mailService)
        {
            _mailService = mailService;
        }

        public Task Handle(Envelope<EmailReceived> envelope, CancellationToken cancellationToken = default)
        {
            return _mailService.SetEmailAsRead(envelope.Message.Recipient, envelope.Message.EmailId, cancellationToken);
        }

        public Task Handle(IEnvelope envelope, CancellationToken cancellationToken = default)
            => Handle(new Envelope<EmailReceived>(envelope), cancellationToken);
    }
}