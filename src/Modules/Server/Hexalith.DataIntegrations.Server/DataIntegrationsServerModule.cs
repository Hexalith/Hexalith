namespace Hexalith.DataIntegrations
{
    using Hexalith.Application.Commands;
    using Hexalith.Application.Events;
    using Hexalith.Application.Messages;
    using Hexalith.Application.Repositories;
    using Hexalith.DataIntegrations.Application.CommandHandlers;
    using Hexalith.DataIntegrations.Application.Commands;
    using Hexalith.DataIntegrations.Contracts.Commands;
    using Hexalith.DataIntegrations.Contracts.Events;
    using Hexalith.DataIntegrations.Domain.States;
    using Hexalith.Emails.Contracts.Events;
    using Hexalith.Infrastructure.EfCore.Repositories;
    using Hexalith.Infrastructure.Helpers;
    using Hexalith.Infrastructure.WebServer.Modules;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public sealed class DataIntegrationsServerModule : ServerModule
    {
        private DataIntegrationsSettings? _settings;

        public DataIntegrationsServerModule(IConfiguration configuration, IWebHostEnvironment environment)
            : base(configuration, environment)
        {
        }

        public DataIntegrationsSettings Settings
            => _settings ??= Configuration.GetSettings<DataIntegrationsSettings>();

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            base.Configure(app, env);
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }

        public override void ConfigureMessages(IMessageFactoryBuilder messageBuilder)
        {
            messageBuilder.AddAssemblyMessages(typeof(SubmitDataIntegration).Assembly);
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            services.ConfigureSettings<DataIntegrationsSettings>(Configuration);
            services.AddTransient<IRepository<IDataIntegrationState>, EfRepository<IDataIntegrationState, DataIntegrationState>>();
            services.AddTransient<IEventHandler<EmailReceived>, EmailReceivedHandler>();
            services.AddTransient<IEventHandler<DataIntegrationSubmitted>, DataIntegrationSubmittedHandler>();
            services.AddTransient<ICommandHandler<SubmitDataIntegration>, SubmitDataIntegrationHandler>();
            services.AddTransient<ICommandHandler<NormalizeDataIntegration>, NormalizeDataIntegrationHandler>();
        }
    }
}