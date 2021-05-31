namespace Hexalith.ApplicationLayer.Helpers
{
    using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
    using Microsoft.Extensions.DependencyInjection;

    public static class ApplicationLayerHelper
    {
#pragma warning disable IDE0060 // Remove unused parameter

        public static void AddApplicationLayer(this IServiceCollection services)
#pragma warning restore IDE0060 // Remove unused parameter
        {
        }

        public static void AddApplicationLayer(this WebAssemblyHostBuilder builder)
        {
            builder.Services.AddApplicationLayer();
        }
    }
}