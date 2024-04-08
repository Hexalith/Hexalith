// <copyright file="WebAssemblyClientHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.ClientAppOnWasm.Helpers;

using System.Globalization;

using Hexalith.Infrastructure.ClientApp.Helpers;

using HexalithApplication.Client;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Serilog;

/// <summary>
/// Helper class for configuring and creating a WebAssembly client for Hexalith application.
/// </summary>
public static class WebAssemblyClientHelper
{
    /// <summary>
    /// Adds the Hexalith WebAssembly client application services to the specified service collection.
    /// </summary>
    /// <param name="services">The service collection to add the services to.</param>
    /// <param name="configuration">The configuration for the client application.</param>
    /// <param name="baseAddress">The base address of the client application.</param>
    /// <returns>The modified service collection.</returns>
    public static IServiceCollection AddHexalithWasmClientApp(this IServiceCollection services, IConfiguration configuration, Uri baseAddress)
    {
        _ = services.AddHexalithClientApp(configuration);
        _ = services
            .AddAuthorizationCore()
            .AddCascadingAuthenticationState()
            .AddHttpClient(
                ClientConstants.FrontApiName,
                client => client.BaseAddress = baseAddress)
            .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();
        _ = services
            .AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
            .CreateClient(ClientConstants.FrontApiName))
            .AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

        return services;
    }

    /// <summary>
    /// Creates a WebAssembly host builder for the Hexalith WebAssembly client.
    /// </summary>
    /// <param name="args">The command-line arguments.</param>
    /// <returns>The WebAssembly host builder.</returns>
    public static WebAssemblyHostBuilder CreateHexalithWasmClient(string[]? args = null)
    {
        WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.WithProperty("InstanceId", Guid.NewGuid().ToString("n"))
            .WriteTo.BrowserConsole(formatProvider: CultureInfo.InvariantCulture)
            .CreateLogger();
        _ = builder.Services
            .AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true))
            .AddHexalithWasmClientApp(builder.Configuration, new Uri(builder.HostEnvironment.BaseAddress));
        return builder;
    }
}