namespace Hexalith.Units
{
    using Hexalith.Application.Messages;
    using Hexalith.Infrastructure.WebServer.Modules;
    using Hexalith.Units.Application.Queries;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;

    public sealed class UnitsServerModule : ServerModule
    {
        public UnitsServerModule(IConfiguration configuration, IWebHostEnvironment environment) : base(configuration, environment)
        {
        }

        public override void ConfigureMessages(IMessageFactoryBuilder messageBuilder)
        {
            messageBuilder.AddAssemblyMessages(typeof(GetAllUnitIds).Assembly);
        }
    }
}