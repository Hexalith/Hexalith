namespace Hexalith.MudBlazorTheme
{
    using System;

    using Hexalith.Application.Messages;
    using Hexalith.Application.Queries;
    using Hexalith.Infrastructure.Helpers;
    using Hexalith.Infrastructure.WebServer.Modules;
    using Hexalith.MudBlazorTheme.Projections.Handlers;
    using Hexalith.MudBlazorTheme.Queries;
    using Hexalith.MudBlazorTheme.Settings;
    using Hexalith.MudBlazorTheme.ViewModels;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public sealed class MudBlazorThemeServerModule : ServerModule
    {
        private MudBlazorThemeSettings? _settings;

        public MudBlazorThemeServerModule(IConfiguration configuration, IWebHostEnvironment environment) : base(configuration, environment)
        {
        }

        public MudBlazorThemeSettings Settings => _settings ??= Configuration.GetSettings<MudBlazorThemeSettings>();

        public override void ConfigureMessages(IMessageFactoryBuilder messageBuilder)
        {
            _ = messageBuilder ?? throw new ArgumentNullException(nameof(messageBuilder));
            messageBuilder.AddAssemblyMessages(typeof(MudBlazorThemeSettings).Assembly);
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            services.ConfigureSettings<MudBlazorThemeSettings>(Configuration);
            services.AddTransient<IQueryHandler<GetMudBlazorThemeSetup, MudBlazorThemeSetup>, GetMudBlazorThemeSetupHandler>();
        }
    }
}