namespace Hexalith.UblDocuments
{
    using Hexalith.Application.Events;
    using Hexalith.Application.Messages;
    using Hexalith.Application.Repositories;
    using Hexalith.DataIntegrations.Contracts.Events;
    using Hexalith.Infrastructure.EfCore.Repositories;
    using Hexalith.Infrastructure.Helpers;
    using Hexalith.Infrastructure.WebServer.Modules;
    using Hexalith.UblDocuments.Application.Events;
    using Hexalith.UblDocuments.Domain.States;
    using Hexalith.UblDocuments.Events;
    using Hexalith.UblDocuments.Infrastructure;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public sealed class UblDocumentsServerModule : ServerModule
    {
#pragma warning disable IDE0052 // Remove unread private members
        private readonly UblDocumentsSettings _settings;
#pragma warning restore IDE0052 // Remove unread private members

        public UblDocumentsServerModule(IConfiguration configuration, IWebHostEnvironment environment)
            : base(configuration, environment)
        {
            _settings = configuration.GetSettings<UblDocumentsSettings>();
        }

        public override void ConfigureMessages(IMessageFactoryBuilder messageBuilder)
        {
            messageBuilder.AddAssemblyMessages(typeof(UblInvoiceSubmitted).Assembly);
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            // services.AddDbContext<UblDocumentsDbContext>(o => o.UseSqlServer(_settings.ConnectionString));
            services.AddTransient<IEventHandler<DataIntegrationSubmitted>, DataIntegrationSubmittedHandler>();
            services.AddTransient<IRepository<IUblInvoiceState>, EfRepository<IUblInvoiceState, UblInvoiceState>>();
        }
    }
}