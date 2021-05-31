namespace Hexalith.Roles
{
    using Hexalith.Application.Messages;
    using Hexalith.Infrastructure.Helpers;
    using Hexalith.Infrastructure.WebServer.Modules;
    using Hexalith.Roles.Application.Queries;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;

    public sealed class RolesServerModule : ServerModule
    {
        private RolesSettings? _settings;

        public RolesServerModule(IConfiguration configuration, IWebHostEnvironment environment) : base(configuration, environment)
        {
        }

        public RolesSettings Settings => _settings ??= Configuration.GetSettings<RolesSettings>();

        public override void ConfigureMessages(IMessageFactoryBuilder messageBuilder)
        {
            messageBuilder.AddAssemblyMessages(typeof(GetRoleDetails).Assembly);
        }
    }
}