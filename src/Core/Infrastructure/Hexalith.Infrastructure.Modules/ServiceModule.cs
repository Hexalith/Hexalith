namespace Hexalith.Infrastructure.Modules
{
    using Microsoft.Extensions.Configuration;

    public abstract class ServiceModule : ModuleBase
    {
        protected ServiceModule(IConfiguration configuration) : base(ModuleType.Server)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
    }
}