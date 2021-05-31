namespace Hexalith.Infrastructure.WebServer.Modules
{
    using System;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;

    public class ServerModuleBuilder
    {
        private IConfiguration? _configuration;
        private IWebHostEnvironment? _environment;

        public ServerModuleBuilder AddConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
            return this;
        }

        public ServerModuleBuilder AddEnvironment(IWebHostEnvironment environment)
        {
            _environment = environment;
            return this;
        }

        public TModule Build<TModule>()
            where TModule : ServerModule, new()
        {
            TModule module = new();
            module.Configuration = _configuration ?? throw new ArgumentException("The configuration argument is not defined. Use AddConfiguration method to set this argument.");
            module.Environment = _environment ?? throw new ArgumentException("The environment argument is not defined. Use AddEnvironment method to set this argument.");
            return module;
        }
    }
}