namespace Hexalith.Users
{
    using Hexalith.Application.Messages;
    using Hexalith.Infrastructure;
    using Hexalith.Infrastructure.WebServer.Modules;
    using Hexalith.Users.Application.Queries;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public sealed class UsersServerModule : ServerModule
    {
        public UsersServerModule(IConfiguration configuration, IWebHostEnvironment environment) : base(configuration, environment)
        {
        }

        public override void ConfigureMessages(IMessageFactoryBuilder messageBuilder)
        {
            messageBuilder.AddAssemblyMessages(typeof(GetAllUserIds).Assembly);
        }
    }
}