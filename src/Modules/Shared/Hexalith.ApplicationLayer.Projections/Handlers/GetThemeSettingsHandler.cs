namespace Hexalith.ApplicationLayer.Projections.Handlers
{
    using System.Threading;
    using System.Threading.Tasks;

    using Hexalith.Application.Messages;
    using Hexalith.Application.Queries;
    using Hexalith.Application.Repositories;
    using Hexalith.ApplicationLayer.Model;
    using Hexalith.ApplicationLayer.Queries;

    public class GetThemeSettingsHandler : QueryHandler<GetThemeSettings, UserInterfaceSettingsDetails>
    {
        public GetThemeSettingsHandler(IRepository<UserSettingsState> repository)
        {
            _settings = settings;
        }

        public override Task<string> Handle(Envelope<UserInterfaceSettingsDetails> envelope, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_settings.Value.ApplicationName);
        }
    }
}