namespace Hexalith.Emails.Application.CommandHandlers
{
    using System.Threading;
    using System.Threading.Tasks;

    using Hexalith.Application.Commands;
    using Hexalith.Application.Messages;
    using Hexalith.Domain.ValueTypes;
    using Hexalith.Emails.Application.Commands;
    using Hexalith.Emails.Application.Services;

    [CommandHandler(Command = typeof(ReceiveUnreadEmails))]
    public class ReceiveUnreadEmailsHandler : ICommandHandler<ReceiveUnreadEmails>
    {
        private readonly ICommandBus _commandBus;
        private readonly IMailboxService _mailService;

        public ReceiveUnreadEmailsHandler(
            ICommandBus commandBus,
            IMailboxService mailService)
        {
            _commandBus = commandBus;
            _mailService = mailService;
        }

        public Task Handle(IEnvelope envelope, CancellationToken cancellationToken = default)
             => Handle(new Envelope<ReceiveUnreadEmails>(envelope), cancellationToken);

        public async Task Handle(Envelope<ReceiveUnreadEmails> envelope, CancellationToken cancellationToken = default)
        {
            foreach (var message in await _mailService.GetUserMails(envelope.Message.Recipient, true, cancellationToken))
            {
                await _commandBus.Send(new Envelope<ReceiveEmail>(message, new MessageId(), envelope), cancellationToken);
            }
        }
    }
}