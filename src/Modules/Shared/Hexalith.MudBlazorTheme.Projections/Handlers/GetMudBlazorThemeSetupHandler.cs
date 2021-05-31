namespace Hexalith.MudBlazorTheme.Projections.Handlers
{
    using System.Threading;
    using System.Threading.Tasks;

    using Hexalith.Application.Messages;
    using Hexalith.Application.Queries;
    using Hexalith.MudBlazorTheme.Queries;
    using Hexalith.MudBlazorTheme.Settings;
    using Hexalith.MudBlazorTheme.ViewModels;

    using Microsoft.Extensions.Options;

    public class GetMudBlazorThemeSetupHandler : QueryHandler<GetMudBlazorThemeSetup, MudBlazorThemeSetup>
    {
        private readonly IOptions<MudBlazorThemeSettings> _settings;

        public GetMudBlazorThemeSetupHandler(IOptions<MudBlazorThemeSettings> settings)
        {
            _settings = settings;
        }

        public override Task<MudBlazorThemeSetup> Handle(Envelope<GetMudBlazorThemeSetup> envelope, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new MudBlazorThemeSetup { BaseColor = _settings.Value.BaseColor });
        }
    }
}