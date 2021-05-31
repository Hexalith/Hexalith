namespace Hexalith.Client
{
    using System;
    using System.Threading.Tasks;

    using Hexalith.ApplicationLayer.Helpers;
    using Hexalith.Infrastructure;
    using Hexalith.Infrastructure.BlazorClient;
    using Hexalith.Infrastructure.Client;
    using Hexalith.Infrastructure.VisualComponents.MudBlazor.Helpers;
    using Hexalith.Infrastructure.VisualComponents.Renderers;

    using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
    using Microsoft.Extensions.DependencyInjection;

    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            if (builder.HostEnvironment.IsDevelopment())
            {
                Console.WriteLine("Blazor client strating in development mode.");
            }
            else
            {
                Console.WriteLine("Blazor client strating in production mode.");
            }

            builder.Services.AddHexalithClient(builder.HostEnvironment, typeof(Program).Namespace ?? string.Empty, HexalithConstants.ServerApiName);
            builder.Logging.AddHexalithClient();
            builder.AddMudBlazorThemeClient();
            builder.AddApplicationLayer();
            builder.Services.AddSingleton<IMenuService, MenuService>();
            builder.Services.AddSingleton<IComponentRendererProvider, ComponentRendererProvider>();
            //builder.Services.AddSingleton<IIconRenderer, LineAwesomeIconRenderer>();
            //builder.Services.AddSingleton<IComponentRenderer, MudBlazorRenderer>();
            await builder.Build().RunAsync();
        }
    }
}