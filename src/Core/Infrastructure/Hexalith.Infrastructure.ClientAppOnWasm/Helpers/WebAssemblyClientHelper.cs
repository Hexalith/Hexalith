// <copyright file="WebAssemblyClientHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.ClientAppOnWasm.Helpers;

using Hexalith.Infrastructure.ClientApp.Helpers;
using Hexalith.Infrastructure.ClientApp.Security;

using HexalithApplication.Client;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Serilog;

public static class WebAssemblyClientHelper
{
    public static IServiceCollection AddHexalithWasmClientApp(this IServiceCollection services, IConfiguration configuration, Uri baseAddress)
    {
        _ = services.AddHexalithClientApp(configuration);
        _ = services
            .AddAuthorizationCore()
            .AddCascadingAuthenticationState()
            .AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>()
            .AddHttpClient(
                ClientConstants.FrontApiName,
                client => client.BaseAddress = baseAddress)
            .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();
        _ = services
            .AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
            .CreateClient(ClientConstants.FrontApiName));
        return services;
    }

    public static WebAssemblyHostBuilder CreateHexalithWasmClient()
    {
        WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault();
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.WithProperty("InstanceId", Guid.NewGuid().ToString("n"))
            .WriteTo.BrowserConsole()
            .CreateLogger();
        _ = builder.Services
            .AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true))
            .AddHexalithWasmClientApp(builder.Configuration, new Uri(builder.HostEnvironment.BaseAddress));
    }
}