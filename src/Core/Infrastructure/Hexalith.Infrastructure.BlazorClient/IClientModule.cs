using Hexalith.Infrastructure.Modules;

using Microsoft.Extensions.DependencyInjection;

namespace Hexalith.Infrastructure.BlazorClient
{
    public interface IClientModule : IModule
    {
        void ConfigureServices(IServiceCollection services);
    }
}