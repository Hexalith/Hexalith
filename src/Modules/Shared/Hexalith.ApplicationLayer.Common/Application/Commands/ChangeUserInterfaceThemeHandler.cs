namespace Hexalith.ApplicationLayer.Application.Commands
{
    using System.Threading;
    using System.Threading.Tasks;

    using Hexalith.Application.Commands;
    using Hexalith.Application.Messages;
    using Hexalith.Application.Repositories;
    using Hexalith.ApplicationLayer.Commands;
    using Hexalith.ApplicationLayer.Domain;

    using Microsoft.Extensions.Logging;

    [CommandHandler(Command = typeof(ChangeUserInterfaceTheme))]
    public class ChangeUserInterfaceThemeHandler : CommandHandler<ChangeUserInterfaceTheme>
    {
        private readonly ILogger<ChangeUserInterfaceThemeHandler> _logger;
        private readonly IRepository<UserSettingsState> _repository;

        public ChangeUserInterfaceThemeHandler(IRepository<UserSettingsState> repository, ILogger<ChangeUserInterfaceThemeHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public override async Task Handle(Envelope<ChangeUserInterfaceTheme> envelope, CancellationToken cancellationToken = default)
        {
            if (!await _repository.Exists(envelope.Message.UserName, cancellationToken))
            {
            }
        }
    }
}