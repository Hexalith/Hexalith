namespace Hexalith.Emails.Application.CommandHandlers
{
    using System.Threading;
    using System.Threading.Tasks;

    using Hexalith.Application.Commands;
    using Hexalith.Application.Messages;
    using Hexalith.Application.Repositories;
    using Hexalith.Domain.ValueTypes;
    using Hexalith.Emails.Application.Commands;
    using Hexalith.Emails.Application.Services;
    using Hexalith.Emails.Domain.States;

    [CommandHandler(Command = typeof(ReceiveAllEmails))]
    public class ReceiveAllEmailsHandler : ICommandHandler<ReceiveAllEmails>
    {
        private readonly ICommandBus _commandBus;
        private readonly IMailboxService _mailService;
        private readonly IRepository<IEmailState> _repository;

        public ReceiveAllEmailsHandler(ICommandBus commandBus, IMailboxService mailService, IRepository<IEmailState> repository)
        {
            _commandBus = commandBus;
            _mailService = mailService;
            _repository = repository;
        }

        public async Task Handle(Envelope<ReceiveAllEmails> envelope, CancellationToken cancellationToken = default)
        {
            foreach (var message in await _mailService.GetUserMails(envelope.Message.Recipient, false, cancellationToken))
            {
                if (!await _repository.Exists(message.EmailId, cancellationToken))
                {
                    await _commandBus.Send(new Envelope<ReceiveEmail>(message, new MessageId(), envelope), cancellationToken);
                }
            }
        }

        public Task Handle(IEnvelope envelope, CancellationToken cancellationToken = default)
            => Handle(new Envelope<ReceiveAllEmails>(envelope), cancellationToken);
    }
}