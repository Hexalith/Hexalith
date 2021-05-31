namespace Hexalith.ApplicationLayer.Projections.Handlers
{
    using System.Threading.Tasks;

    using Hexalith.Application.Messages;
    using Hexalith.Application.Queries;
    using Hexalith.ApplicationLayer.Settings;
    using Hexalith.ApplicationLayer.Queries;

    using Microsoft.Extensions.Options;
    using System.Threading;

    public class GetApplicationNameHandler : QueryHandler<GetApplicationName, string>
    {
        private readonly IOptions<ApplicationLayerSettings> _settings;

        public GetApplicationNameHandler(IOptions<ApplicationLayerSettings> settings)
        {
            _settings = settings;
        }

        public override Task<string> Handle(Envelope<GetApplicationName> envelope, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_settings.Value.ApplicationName);
        }
    }
}