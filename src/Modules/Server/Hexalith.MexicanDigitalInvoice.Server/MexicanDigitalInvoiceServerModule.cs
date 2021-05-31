namespace Hexalith.MexicanDigitalInvoice
{
    using Hexalith.Application.Events;
    using Hexalith.Application.Messages;
    using Hexalith.Application.Repositories;
    using Hexalith.DataIntegrations.Contracts.Events;
    using Hexalith.Infrastructure.EfCore.Repositories;
    using Hexalith.Infrastructure.Helpers;
    using Hexalith.Infrastructure.WebServer.Modules;
    using Hexalith.MexicanDigitalInvoice.Application.Events;
    using Hexalith.MexicanDigitalInvoice.Domain.States;
    using Hexalith.MexicanDigitalInvoice.Events;
    using Hexalith.MexicanDigitalInvoice.Infrastructure;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public sealed class MexicanDigitalInvoiceServerModule : ServerModule
    {
#pragma warning disable IDE0052 // Remove unread private members
        private readonly MexicanDigitalInvoiceSettings _settings;
#pragma warning restore IDE0052 // Remove unread private members

        public MexicanDigitalInvoiceServerModule(IConfiguration configuration, IWebHostEnvironment environment)
            : base(configuration, environment)
        {
            _settings = configuration.GetSettings<MexicanDigitalInvoiceSettings>();
        }

        public override void ConfigureMessages(IMessageFactoryBuilder messageBuilder)
        {
            messageBuilder.AddAssemblyMessages(typeof(MexicanDigitalInvoiceSubmitted).Assembly);
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IEventHandler<DataIntegrationSubmitted>, DataIntegrationSubmittedHandler>();
            services.AddTransient<IRepository<IMexicanDigitalInvoiceState>, EfRepository<IMexicanDigitalInvoiceState, MexicanDigitalInvoiceState>>();
        }
    }
}