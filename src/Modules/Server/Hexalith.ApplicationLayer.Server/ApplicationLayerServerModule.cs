namespace Hexalith.ApplicationLayer
{
    using System;

    using Hexalith.Application.Messages;
    using Hexalith.Application.Queries;
    using Hexalith.ApplicationLayer.Projections.Handlers;
    using Hexalith.ApplicationLayer.Queries;
    using Hexalith.ApplicationLayer.Settings;
    using Hexalith.Infrastructure.Helpers;
    using Hexalith.Infrastructure.WebServer.Modules;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public sealed class ApplicationLayerServerModule : ServerModule
    {
        private ApplicationLayerSettings? _settings;

        public ApplicationLayerServerModule(IConfiguration configuration, IWebHostEnvironment environment) : base(configuration, environment)
        {
        }

        public ApplicationLayerSettings Settings => _settings ??= Configuration.GetSettings<ApplicationLayerSettings>();

        public override void ConfigureMessages(IMessageFactoryBuilder messageBuilder)
        {
            _ = messageBuilder ?? throw new ArgumentNullException(nameof(messageBuilder));
            messageBuilder.AddAssemblyMessages(typeof(ApplicationLayerSettings).Assembly);
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            services.ConfigureSettings<ApplicationLayerSettings>(Configuration);
            services.AddTransient<IQueryHandler<GetApplicationName, string>, GetApplicationNameHandler>();
        }
    }
}