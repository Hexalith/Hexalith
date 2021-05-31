namespace Hexalith.MicrosoftIdentity
{
    using Hexalith.Infrastructure.Helpers;
    using Hexalith.Infrastructure.WebServer.Modules;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Identity.Web;

    public class MicrosoftIdentityServerModule : ServerModule
    {
        private MicrosoftIdentitySettings? _settings;

        public MicrosoftIdentityServerModule(IConfiguration configuration, IWebHostEnvironment environment) : base(configuration, environment)
        {
        }

        public MicrosoftIdentitySettings Settings => _settings ??= Configuration.GetSettings<MicrosoftIdentitySettings>();

        public override void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureSettings<MicrosoftIdentitySettings>(Configuration);

            if (!string.IsNullOrWhiteSpace(Settings.AzureAd.ClientId))
            {
                services
                    .AddMicrosoftIdentityWebAppAuthentication(
                            Configuration,
                            nameof(MicrosoftIdentitySettings) + ":" + nameof(MicrosoftIdentitySettings.AzureAd))
                    .EnableTokenAcquisitionToCallDownstreamApi(Settings.MicrosoftGraph?.Scopes?.Split(';'))
                    .AddMicrosoftGraph("https://graph.microsoft.com/beta",
                        "User.ReadBasic.All user.read")
                    .AddInMemoryTokenCaches();
            }
        }
    }
}