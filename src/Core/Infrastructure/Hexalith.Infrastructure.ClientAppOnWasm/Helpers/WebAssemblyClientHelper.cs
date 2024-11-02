// <copyright file="WebAssemblyClientHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.ClientAppOnWasm.Helpers;

using System.Diagnostics.CodeAnalysis;
using System.Globalization;

using Blazored.SessionStorage;

using Hexalith.Application;
using Hexalith.Application.Commands;
using Hexalith.Application.Modules.Applications;
using Hexalith.Infrastructure.ClientApp;
using Hexalith.Infrastructure.ClientApp.Helpers;
using Hexalith.Infrastructure.ClientApp.Services;
using Hexalith.Infrastructure.ClientAppOnWasm.Services;

using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

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
        _ = services.AddScoped<ICommandService, ClientCommandService>();
        _ = services
            .AddAuthorizationCore()
            .AddHttpClient(
                ClientConstants.FrontApiName,
                client => client.BaseAddress = baseAddress)
            .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

        _ = services
            .AddScoped<CustomAuthenticationStateProvider>()
            .AddScoped<ISessionManager, SessionManager>()
            .AddScoped<ISessionIdService, SessionIdService>()
            .AddBlazoredSessionStorage()
            .AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
            .CreateClient(ClientConstants.FrontApiName));
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
        HexalithApplication.AddClientServices(builder.Services, builder.Configuration);
        return builder;
    }

    /// <summary>
    /// Uses the user-defined culture for the WebAssembly host.
    /// </summary>
    /// <param name="host">The WebAssembly host.</param>
    /// <returns><see cref="Task"/> representing the asynchronous operation.</returns>
    public static async Task UseHexalithUserDefinedCultureAsync([NotNull] this WebAssemblyHost host)
    {
        string defaultCulture = CultureInfo.CurrentCulture.Name;

        IJSRuntime js = host.Services.GetRequiredService<IJSRuntime>();
        string? result = await js.InvokeAsync<string>(ApplicationConstants.UserDefinedCulturePropertyName + ".get").ConfigureAwait(false);
        CultureInfo culture = CultureInfo.GetCultureInfo(result ?? defaultCulture);

        if (result == null)
        {
            await js.InvokeVoidAsync(ApplicationConstants.UserDefinedCulturePropertyName + ".set", defaultCulture).ConfigureAwait(false);
        }

        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
    }
}