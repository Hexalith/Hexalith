namespace Hexalith.ApplicationLayer.UserInterfaceTheme
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using Hexalith.Application.Messages;
    using Hexalith.Application.Queries;
    using Hexalith.Application.Repositories;
    using Hexalith.ApplicationLayer.Model;
    using Hexalith.ApplicationLayer.Queries;

    using Microsoft.Extensions.Logging;

    public class GetUserInterfaceThemeHandler : QueryHandler<GetUserInterfaceTheme, UserInterfaceTheme>
    {
        private readonly ILogger<GetUserInterfaceThemeHandler> _logger;
        private readonly IKeyValueStore<UserInterfaceTheme> _repository;

        public GetUserInterfaceThemeHandler(IKeyValueStore<UserInterfaceTheme> repository, ILogger<GetUserInterfaceThemeHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public override async Task<UserInterfaceTheme> Handle(Envelope<GetUserInterfaceTheme> envelope, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _repository.Get(envelope.Message.UserName);
            }
            catch (KeyNotFoundException _)
            {
                _logger.LogError($"Theme not found for user {envelope.Message.UserName}. MessageId : {envelope.MessageId}");
                throw;
            }
        }
    }
}