﻿// <copyright file="ServerSideClientAppHelper.cs" company="ITANEO">
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
using Hexalith.Domain.Events;
using Hexalith.Domains.Abstractions.Extensions;
using Hexalith.Domains.ValueObjects;
using Hexalith.Infrastructure.ClientAppOnServer.Services;
using Hexalith.Infrastructure.DaprRuntime.Handlers;
using Hexalith.Infrastructure.DaprRuntime.Helpers;
using Hexalith.Infrastructure.DaprRuntime.Partitions.Helpers;
using Hexalith.Infrastructure.DaprRuntime.Services;
using Hexalith.Infrastructure.DaprRuntime.Sessions.Helpers;
using Hexalith.Infrastructure.Emails.SendGrid.Helpers;
using Hexalith.Infrastructure.WebApis.Controllers;
using Hexalith.Infrastructure.WebApis.Helpers;
using Hexalith.NetAspire.Defaults;
using Hexalith.PolymorphicSerializations;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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
        HexalithApplicationAbstractionsSerialization.RegisterPolymorphicMappers();
        HexalithDomainsAbstractionsSerialization.RegisterPolymorphicMappers();

        _ = services
            .AddOrganizations(configuration)
            .AddSendGridEmail(configuration)
            .AddSingleton(TimeProvider.System)
            .AddSingleton<IRouteManager, RouteManager>()
            .AddScoped<ICommandService, ServerCommandService>()
            .AddScoped<IRequestService, WebServerRequestService>()
            .AddScoped<ISessionService, WebServerSessionService>()
            .AddScoped<IRequestProcessor, DependencyInjectionRequestProcessor>()
            .AddPartitions(configuration)
            .AddSessionsServices();
        _ = services.AddValidatorsFromAssemblyContaining<CommandBusSettingsValidator>(
            ServiceLifetime.Singleton);
        services.TryAddSingleton<IResiliencyPolicyProvider, ResiliencyPolicyProvider>();
        services.TryAddScoped<
            IDomainCommandDispatcher,
            DependencyInjectionDomainCommandDispatcher
        >();
        services.TryAddScoped<
            IProjectionUpdateProcessor,
            DependencyInjectionProjectionUpdateProcessor
        >();
        services.TryAddSingleton<IDomainAggregateFactory, DomainAggregateFactory>();
        services.TryAddScoped<
            IProjectionUpdateHandler<SnapshotEvent>,
            IdsCollectionProjectionHandler<SnapshotEvent>
        >();
        services.TryAddSingleton<IIdCollectionFactory, IdCollectionFactory>();
        services.TryAddSingleton<IDomainCommandProcessor>(
            (s) =>
                new DomainActorCommandProcessor(
                    ActorProxy.DefaultProxyFactory,
                    false,
                    s.GetRequiredService<ILogger<DomainActorCommandProcessor>>()));
        _ = services.AddActorProjectionFactory<IdDescription>();
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

        // Enable Static Web Assets for non-development environments during testing
        if (!builder.Environment.IsDevelopment())
        {
            _ = builder.WebHost.UseStaticWebAssets();
        }

        _ = builder.Services.AddCertificateForwarding(options => options.CertificateHeader = "X-ARR-ClientCert");
        _ = builder.Services.AddHttpLogging(options => options.LoggingFields = HttpLoggingFields.RequestPropertiesAndHeaders);
        _ = builder.Services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;

            // These clear calls let any proxy be trusted; in production you can tighten this
            options.KnownNetworks.Clear();
            options.KnownProxies.Clear();
        });
        _ = builder.Services.AddHttpClient();

        builder.Services.AddDaprClient();

        builder.Services.AddActors(options =>
        {
            options.ActorIdleTimeout = builder.Environment.IsDevelopment()
                ? TimeSpan.FromMinutes(3)
                : TimeSpan.FromMinutes(1);

            // Register actor types and configure actor settings
            registerActors(options.Actors);
        });

        startupLogger.Information("Configuring {AppName} ...", applicationName);
        _ = builder
            .AddServiceDefaults()
            .Services.AddLocalization()
            .AddProblemDetails()
            .AddHexalithServerSideClientApp(builder.Configuration)
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(c =>
                c.SwaggerDoc("v1", new() { Title = applicationName, Version = version }))
            .AddDaprAggregateServices()
            .AddDaprBuses(builder.Configuration)
            .AddDaprStateStore(builder.Configuration);

        _ = builder
            .Services.ConfigureHttpJsonOptions(options => options.SerializerOptions.SetDefault())
            .AddHttpContextAccessor()
            .AddControllers()
            .AddApplicationPart(typeof(RequestServiceController).Assembly)
            .AddDapr(dapr =>
            {
                if (builder.Environment.IsDevelopment())
                {
                    _ = dapr.UseTimeout(TimeSpan.FromMinutes(1));
                }
            });

        _ = builder
            .Services.AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddInteractiveWebAssemblyComponents()
            .AddAuthenticationStateSerialization();

        _ = builder.Services.AddDistributedMemoryCache()
            .AddSession(options =>
        {
            options.Cookie.Name = sessionCookieName;
            options.IdleTimeout = TimeSpan.FromMinutes(20);
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
        options.AllowTrailingCommas = PolymorphicHelper
            .DefaultJsonSerializerOptions
            .AllowTrailingCommas;
        foreach (
            System.Text.Json.Serialization.JsonConverter converter in PolymorphicHelper
                .DefaultJsonSerializerOptions
                .Converters)
        {
            options.Converters.Add(converter);
        }

        options.DefaultBufferSize = PolymorphicHelper
            .DefaultJsonSerializerOptions
            .DefaultBufferSize;
        options.DefaultIgnoreCondition = PolymorphicHelper
            .DefaultJsonSerializerOptions
            .DefaultIgnoreCondition;
        options.DictionaryKeyPolicy = PolymorphicHelper
            .DefaultJsonSerializerOptions
            .DictionaryKeyPolicy;
        options.Encoder = PolymorphicHelper.DefaultJsonSerializerOptions.Encoder;
        options.IgnoreReadOnlyFields = PolymorphicHelper
            .DefaultJsonSerializerOptions
            .IgnoreReadOnlyFields;
        options.IgnoreReadOnlyProperties = PolymorphicHelper
            .DefaultJsonSerializerOptions
            .IgnoreReadOnlyProperties;
        options.IncludeFields = PolymorphicHelper.DefaultJsonSerializerOptions.IncludeFields;
        options.MaxDepth = PolymorphicHelper.DefaultJsonSerializerOptions.MaxDepth;
        options.NumberHandling = PolymorphicHelper.DefaultJsonSerializerOptions.NumberHandling;
        options.PreferredObjectCreationHandling = PolymorphicHelper
            .DefaultJsonSerializerOptions
            .PreferredObjectCreationHandling;
        options.PropertyNameCaseInsensitive = PolymorphicHelper
            .DefaultJsonSerializerOptions
            .PropertyNameCaseInsensitive;
        options.PropertyNamingPolicy = PolymorphicHelper
            .DefaultJsonSerializerOptions
            .PropertyNamingPolicy;
        options.ReadCommentHandling = PolymorphicHelper
            .DefaultJsonSerializerOptions
            .ReadCommentHandling;
        options.ReferenceHandler = PolymorphicHelper.DefaultJsonSerializerOptions.ReferenceHandler;
        options.TypeInfoResolver = PolymorphicHelper.DefaultJsonSerializerOptions.TypeInfoResolver;
        options.UnknownTypeHandling = PolymorphicHelper
            .DefaultJsonSerializerOptions
            .UnknownTypeHandling;
        options.UnmappedMemberHandling = PolymorphicHelper
            .DefaultJsonSerializerOptions
            .UnmappedMemberHandling;
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

        IEnumerable<IApplicationModule>? modules =
            scope.ServiceProvider.GetServices<IApplicationModule>();
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

        IEnumerable<IApplicationModule>? modules =
            scope.ServiceProvider.GetServices<IApplicationModule>();
        foreach (
            IWebServerApplicationModule module in modules.OfType<IWebServerApplicationModule>())
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
    public static IApplicationBuilder UseHexalithWebApplication<TApp>(
        [NotNull] this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        // Configure static assets
        _ = app.UseForwardedHeaders();
        _ = app.MapStaticAssets().AllowAnonymous();
        _ = app.UseStaticFiles();

        _ = app.Use(async (context, next) =>
        {
            // Connection: RemoteIp
            app.Logger.LogInformation(
                "Request RemoteIp: {RemoteIpAddress}",
                context.Connection.RemoteIpAddress);

            await next(context);
        });

        _ = app
            .MapDefaultEndpoints()
            .UseSerilogRequestLogging()
            .UseCloudEvents();

        if (app.Environment.IsDevelopment())
        {
            _ = app.UseDeveloperExceptionPage();
            app.UseWebAssemblyDebugging();
        }
        else
        {
            _ = app.UseExceptionHandler("/Error", createScopeForErrors: true)
                .UseHsts();
        }

        // Needed when behind a reverse proxy like Azure Container Instances. It will forward the original host and protocol (https/http).
        _ = app.UseHttpLogging();

        _ = app.UseRequestLocalization(
            new RequestLocalizationOptions()
                .AddSupportedCultures(_cultures)
                .AddSupportedUICultures(_cultures));

        _ = app.UseSession();

        // Configure authentication and authorization after static files
        app.UseHexalithSecurity();
        _ = app.UseAntiforgery();

        _ = app.UseSwagger().UseSwaggerUI();
        _ = app.MapControllers();
        _ = app.MapSubscribeHandler();
        RazorComponentsEndpointConventionBuilder razor = app.MapRazorComponents<TApp>()
            .AddInteractiveServerRenderMode()
            .AddInteractiveWebAssemblyRenderMode();
        if (HexalithApplication.WebServerApplication is not null)
        {
            System.Reflection.Assembly[]? assemblies = HexalithApplication
                .WebServerApplication?.PresentationAssemblies.Where(assembly =>
                    assembly != typeof(TApp).Assembly)
                .Distinct()
                .ToArray();
            if (assemblies != null)
            {
                _ = razor.AddAdditionalAssemblies(assemblies);
            }
        }

        _ = app.MapActorsHandlers().AllowAnonymous();

        app.UseHexalithModules();

        return app;
    }
}