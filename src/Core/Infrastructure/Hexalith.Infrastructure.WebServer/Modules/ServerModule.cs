namespace Hexalith.Infrastructure.WebServer.Modules
{
    using Hexalith.Application.Messages;
    using Hexalith.Infrastructure.Helpers;
    using Hexalith.Infrastructure.Modules;
    using Hexalith.Infrastructure.WebServer.Settings;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public abstract class ServerModule : ModuleBase, IServerModule
    {
        private IConfiguration _configuration;

        private IWebHostEnvironment _environment;

        protected ServerModule(IConfiguration configuration, IWebHostEnvironment environment)
            : base(ModuleType.Server)
        {
            _configuration = configuration ?? throw new ModuleNotInitializedException(nameof(_configuration));
            _environment = environment ?? throw new ModuleNotInitializedException(nameof(_environment));
        }

        public IConfiguration Configuration
        {
            get => _configuration;
            internal set => _configuration = value;
        }

        public IWebHostEnvironment Environment
        {
            get => _environment;
            internal set => _environment = value;
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
        }

        public virtual void ConfigureMessages(IMessageFactoryBuilder messageBuilder)
        {
        }

        public virtual void ConfigureServices(IServiceCollection services)
        {
        }

        public virtual void OnStarted()
        {
        }

        public virtual void OnStopped()
        {
        }

        public virtual void OnStopping()
        {
        }
    }
}