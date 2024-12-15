// <copyright file="ServerSideClientAppHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.ClientAppOnServer.Helpers;

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

using Dapr.Actors.Client;
using Dapr.Actors.Runtime;

using FluentValidation;

using Hexalith.Application.Abstractions.Extensions;
using Hexalith.Application.Aggregates;
using Hexalith.Application.Buses;
using Hexalith.Application.Commands;
using Hexalith.Application.Modules.Applications;
using Hexalith.Application.Modules.Modules;
using Hexalith.Application.Modules.Routes;
using Hexalith.Application.Organizations.Helpers;
using Hexalith.Application.Projections;
using Hexalith.Application.Requests;
using Hexalith.Application.Services;
using Hexalith.Application.Sessions.Services;
using Hexalith.Application.Tasks;
using Hexalith.Infrastructure.AspireService.Defaults;
using Hexalith.Infrastructure.ClientAppOnServer.Services;
using Hexalith.Infrastructure.DaprRuntime.Handlers;
using Hexalith.Infrastructure.DaprRuntime.Helpers;
using Hexalith.Infrastructure.DaprRuntime.Partitions.Helpers;
using Hexalith.Infrastructure.DaprRuntime.Services;
using Hexalith.Infrastructure.DaprRuntime.Sessions.Helpers;
using Hexalith.Infrastructure.Emails.SendGrid.Helpers;
using Hexalith.Infrastructure.WebApis.Helpers;
using Hexalith.PolymorphicSerialization;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.FluentUI.AspNetCore.Components;

using Serilog;

/// <summary>
/// Server-side client application helper.
/// </summary>
public static class ServerSideClientAppHelper
{
    // TODO Move to a configuration file
    private static readonly string[] _cultures = ["en-US", "fr-FR"];

    /// <summary>
    /// Adds Hexalith server-side client application services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddHexalithServerSideClientApp(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        HexalithApplicationAbstractions.RegisterPolymorphicMappers();
        _ = services
            .AddFluentUIComponents()
            .AddOrganizations(configuration)
            .AddSendGridEmail(configuration)
            .AddSingleton(TimeProvider.System)
            .AddSingleton<IRouteManager, RouteManager>()
            .AddScoped<ICommandService, ServerCommandService>()
            .AddScoped<IRequestService, ServerRequestService>()
            .AddScoped<ISessionService, ServerSessionService>()
            .AddScoped<IRequestProcessor, DependencyInjectionRequestProcessor>()
            .AddPartitions(configuration)
            .AddSessions();
        _ = services.AddValidatorsFromAssemblyContaining<CommandBusSettingsValidator>(ServiceLifetime.Singleton);
        services.TryAddSingleton<IResiliencyPolicyProvider, ResiliencyPolicyProvider>();
        services.TryAddScoped<IDomainCommandDispatcher, DependencyInjectionDomainCommandDispatcher>();
        services.TryAddScoped<IProjectionUpdateProcessor, DependencyInjectionProjectionUpdateProcessor>();
        services.TryAddSingleton<IDomainAggregateFactory, DomainAggregateFactory>();
        services.TryAddSingleton<IIdCollectionFactory, IdCollectionFactory>();
        services
            .TryAddSingleton<IDomainCommandProcessor>((s) => new DomainActorCommandProcessor(
            ActorProxy.DefaultProxyFactory,
            false,
            s.GetRequiredService<ILogger<DomainActorCommandProcessor>>()));
        return services;
    }

    /// <summary>
    /// Creates the server-side client application.
    /// </summary>
    /// <param name="applicationName">The name of the application.</param>
    /// <param name="registerActors">The register actors.</param>
    /// <param name="sessionCookieName">The name of the session cookie.</param>
    /// <param name="version">The version of the application.</param>
    /// <param name="args">The command-line arguments.</param>
    /// <returns>The web application builder.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the connection string 'DefaultConnection' is not found.</exception>
    public static WebApplicationBuilder CreateServerSideClientApplication(
        string applicationName,
        Action<ActorRegistrationCollection> registerActors,
        string sessionCookieName,
        string version,
        string[] args)
    {
        Activity.DefaultIdFormat = ActivityIdFormat.W3C;

        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        Serilog.ILogger startupLogger = builder.AddSerilogLogger();

        _ = builder.Services.AddRazorPages();
        _ = builder.Services.AddServerSideBlazor();
        builder.Services.AddDaprClient();

        builder.Services.AddActors(options =>

            // Register actor types and configure actor settings
            registerActors(options.Actors));

        startupLogger.Information("Configuring {AppName} ...", applicationName);
        _ = builder
            .AddServiceDefaults()
            .Services
            .AddLocalization(options => options.ResourcesPath = "Resources")
            .AddProblemDetails()
            .AddHexalithServerSideClientApp(builder.Configuration)
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(c => c.SwaggerDoc("v1", new() { Title = applicationName, Version = version, }))
            .AddDaprBuses(builder.Configuration)
            .AddDaprStateStore(builder.Configuration);

        _ = builder
            .Services
            .ConfigureHttpJsonOptions(options => options.SerializerOptions.SetDefault())
            .AddHttpContextAccessor()
            .AddControllers()
            .AddDapr(dapr =>
            {
                if (builder.Environment.IsDevelopment())
                {
                    _ = dapr.UseTimeout(TimeSpan.FromMinutes(1));
                }
            });

        _ = builder.Services
            .AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddInteractiveWebAssemblyComponents()
            .AddAuthenticationStateSerialization();

        // Show detailed errors on Circuit exceptions
        _ = builder.Services.AddServerSideBlazor().AddCircuitOptions(option => option.DetailedErrors = true);

#pragma warning disable EXTEXP0018 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        _ = builder.Services
            .AddDistributedMemoryCache()
            .AddHybridCache();
#pragma warning restore EXTEXP0018 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        _ = builder.Services
            .AddSession(options =>
            {
                options.Cookie.Name = sessionCookieName;
                options.IdleTimeout = TimeSpan.FromHours(1);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
        HexalithApplication.AddWebServerServices(builder.Services, builder.Configuration);
        return builder;
    }

    /// <summary>
    /// Sets the default JSON options.
    /// </summary>
    /// <param name="options">The json options to default.</param>
    /// <returns>The defaulted JSON options.</returns>
    public static JsonSerializerOptions SetDefault(this JsonSerializerOptions options)
    {
        options.AllowTrailingCommas = PolymorphicHelper.DefaultJsonSerializerOptions.AllowTrailingCommas;
        foreach (System.Text.Json.Serialization.JsonConverter converter in PolymorphicHelper.DefaultJsonSerializerOptions.Converters)
        {
            options.Converters.Add(converter);
        }

        options.DefaultBufferSize = PolymorphicHelper.DefaultJsonSerializerOptions.DefaultBufferSize;
        options.DefaultIgnoreCondition = PolymorphicHelper.DefaultJsonSerializerOptions.DefaultIgnoreCondition;
        options.DictionaryKeyPolicy = PolymorphicHelper.DefaultJsonSerializerOptions.DictionaryKeyPolicy;
        options.Encoder = PolymorphicHelper.DefaultJsonSerializerOptions.Encoder;
        options.IgnoreReadOnlyFields = PolymorphicHelper.DefaultJsonSerializerOptions.IgnoreReadOnlyFields;
        options.IgnoreReadOnlyProperties = PolymorphicHelper.DefaultJsonSerializerOptions.IgnoreReadOnlyProperties;
        options.IncludeFields = PolymorphicHelper.DefaultJsonSerializerOptions.IncludeFields;
        options.MaxDepth = PolymorphicHelper.DefaultJsonSerializerOptions.MaxDepth;
        options.NumberHandling = PolymorphicHelper.DefaultJsonSerializerOptions.NumberHandling;
        options.PreferredObjectCreationHandling = PolymorphicHelper.DefaultJsonSerializerOptions.PreferredObjectCreationHandling;
        options.PropertyNameCaseInsensitive = PolymorphicHelper.DefaultJsonSerializerOptions.PropertyNameCaseInsensitive;
        options.PropertyNamingPolicy = PolymorphicHelper.DefaultJsonSerializerOptions.PropertyNamingPolicy;
        options.ReadCommentHandling = PolymorphicHelper.DefaultJsonSerializerOptions.ReadCommentHandling;
        options.ReferenceHandler = PolymorphicHelper.DefaultJsonSerializerOptions.ReferenceHandler;
        options.TypeInfoResolver = PolymorphicHelper.DefaultJsonSerializerOptions.TypeInfoResolver;
        options.UnknownTypeHandling = PolymorphicHelper.DefaultJsonSerializerOptions.UnknownTypeHandling;
        options.UnmappedMemberHandling = PolymorphicHelper.DefaultJsonSerializerOptions.UnmappedMemberHandling;
        options.WriteIndented = PolymorphicHelper.DefaultJsonSerializerOptions.WriteIndented;
        return options;
    }

    /// <summary>
    /// Configures the application to use the modules.
    /// </summary>
    /// <param name="application">The application.</param>
    public static void UseHexalithModules([NotNull] this IHost application)
    {
        ArgumentNullException.ThrowIfNull(application);

        // initialize modules
        using IServiceScope scope = application.Services.CreateScope();

        IEnumerable<IApplicationModule>? modules = scope
            .ServiceProvider
            .GetServices<IApplicationModule>();
        foreach (IApplicationModule module in modules)
        {
            module.UseModule(application);
        }
    }

    /// <summary>
    /// Configures the application to use the modules.
    /// </summary>
    /// <param name="application">The application.</param>
    public static void UseHexalithSecurity([NotNull] this IHost application)
    {
        ArgumentNullException.ThrowIfNull(application);

        // initialize modules
        using IServiceScope scope = application.Services.CreateScope();

        IEnumerable<IApplicationModule>? modules = scope
            .ServiceProvider
            .GetServices<IApplicationModule>();
        foreach (IWebServerApplicationModule module in modules.OfType<IWebServerApplicationModule>())
        {
            module.UseSecurity(application);
        }
    }

    /// <summary>
    /// Uses the hexalith web application.
    /// </summary>
    /// <typeparam name="TApp">The type of the t application.</typeparam>
    /// <param name="app">The application.</param>
    /// <returns>IApplicationBuilder.</returns>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public static IApplicationBuilder UseHexalithWebApplication<TApp>([NotNull] this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        _ = app
            .MapDefaultEndpoints()
            .UseSerilogRequestLogging()
            .UseCloudEvents();

        if (!app.Environment.IsProduction())
        {
            app
                 .UseDeveloperExceptionPage()
                 .UseWebAssemblyDebugging();
        }
        else
        {
            _ = app
                .UseExceptionHandler("/Error", createScopeForErrors: true)
                .UseHsts();
        }

        _ = app.UseRequestLocalization(new RequestLocalizationOptions()
          .AddSupportedCultures(_cultures)
          .AddSupportedUICultures(_cultures));
        _ = app
            .MapStaticAssets();
        _ = app
            .UseRouting();
        app.UseHexalithSecurity();
        _ = app.UseAntiforgery();
        _ = app
            .UseSwagger()
            .UseSwaggerUI()
            .UseSession();
        _ = app.MapControllers();
        _ = app.MapSubscribeHandler();
        RazorComponentsEndpointConventionBuilder razor = app
            .MapRazorComponents<TApp>()
            .AddInteractiveServerRenderMode()
            .AddInteractiveWebAssemblyRenderMode();
        if (HexalithApplication.WebServerApplication is not null)
        {
            System.Reflection.Assembly[]? assemblies = HexalithApplication.WebServerApplication?.PresentationAssemblies.ToArray();
            if (assemblies != null)
            {
                _ = razor.AddAdditionalAssemblies(assemblies);
            }
        }

        _ = app.MapActorsHandlers();

        app.UseHexalithModules();

        // _ = app.MapBlazorHub();
        return app;
    }
}