namespace Hexalith.GoogleIdentity
{
    using Hexalith.Infrastructure.Helpers;
    using Hexalith.Infrastructure.WebServer.Modules;

    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class GoogleIdentityServerModule : ServerModule
    {
        private GoogleIdentitySettings? _settings;

        public GoogleIdentityServerModule(IConfiguration configuration, IWebHostEnvironment environment) : base(configuration, environment)
        {
        }

        public GoogleIdentitySettings Settings => _settings ??= Configuration.GetSettings<GoogleIdentitySettings>();

        public override void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureSettings<GoogleIdentitySettings>(Configuration);

            if (!string.IsNullOrWhiteSpace(Settings.ClientId) && !string.IsNullOrWhiteSpace(Settings.ClientSecret))
            {
                services
                    .AddAuthentication(o =>
                        o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme
                    )
                    .AddGoogle(o =>
                    {
                        o.ClientId = Settings.ClientId;
                        o.ClientSecret = Settings.ClientSecret;
                    });
            }
        }
    }
}