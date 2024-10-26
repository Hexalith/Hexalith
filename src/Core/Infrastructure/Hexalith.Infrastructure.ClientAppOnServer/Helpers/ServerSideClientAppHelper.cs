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

using Hexalith.Application.Aggregates;
using Hexalith.Application.Buses;
using Hexalith.Application.Commands;
using Hexalith.Application.Modules.Applications;
using Hexalith.Application.Modules.Modules;
using Hexalith.Application.Projections;
using Hexalith.Application.Tasks;
using Hexalith.Infrastructure.AspireService.Defaults;
using Hexalith.Infrastructure.ClientApp.Helpers;
using Hexalith.Infrastructure.ClientApp.Services;
using Hexalith.Infrastructure.ClientAppOnServer.Services;
using Hexalith.Infrastructure.DaprRuntime.Handlers;
using Hexalith.Infrastructure.DaprRuntime.Helpers;
using Hexalith.Infrastructure.WebApis.Helpers;
using Hexalith.PolymorphicSerialization;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
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
        _ = services.AddScoped<IClientCommandService, ClientCommandService>();
        _ = services.AddScoped<IUserService, UserService>();
        _ = services.AddScoped<ISessionService, SessionService>();
        return services.AddHexalithClientApp(configuration);
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
    /// Creates the server-side client application.
    /// </summary>
    /// <param name="applicationName">The name of the application.</param>
    /// <param name="sessionCookieName">The name of the session cookie.</param>
    /// <param name="version">The version of the application.</param>
    /// <param name="registerActors">The action to register actors.</param>
    /// <param name="args">The command-line arguments.</param>
    /// <returns>The web application builder.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the connection string 'DefaultConnection' is not found.</exception>
    public static WebApplicationBuilder CreateServerSideClientApplication(
        string applicationName,
        string sessionCookieName,
        string version,
        Action<ActorRegistrationCollection> registerActors,
        string[] args)
    {
        Activity.DefaultIdFormat = ActivityIdFormat.W3C;

        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        Serilog.ILogger startupLogger = builder.AddSerilogLogger();

        startupLogger.Information("Configuring {AppName} ...", applicationName);
        builder
            .AddServiceDefaults()
            .Services
            .AddProblemDetails()
            .AddHexalithServerSideClientApp(builder.Configuration)
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(c => c.SwaggerDoc("v1", new() { Title = applicationName, Version = version, }))
            .AddDaprBuses(builder.Configuration)
            .AddDaprStateStore(builder.Configuration)
            .AddDaprClient(dapr =>
            {
                dapr.UseJsonSerializationOptions(PolymorphicHelper.DefaultJsonSerializerOptions);
                if (builder.Environment.IsDevelopment())
                {
                    dapr.UseTimeout(TimeSpan.FromMinutes(1));
                }
            });

        builder.Services.AddActors(options =>
        {
            registerActors(options.Actors);
            options.JsonSerializerOptions = PolymorphicHelper.DefaultJsonSerializerOptions;
        });

        _ = builder
            .Services
            .ConfigureHttpJsonOptions(options => options.SerializerOptions.SetDefault())
            .AddHttpContextAccessor()
            .AddControllers()
            .AddJsonOptions(options => options.JsonSerializerOptions.SetDefault())
            .AddDapr(dapr =>
            {
                dapr.UseJsonSerializationOptions(PolymorphicHelper.DefaultJsonSerializerOptions);
                if (builder.Environment.IsDevelopment())
                {
                    dapr.UseTimeout(TimeSpan.FromMinutes(1));
                }
            });

        _ = builder.Services
            .AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddInteractiveWebAssemblyComponents();
#if DEBUG

        // Show detailed errors on Circuit exceptions
        builder.Services.AddServerSideBlazor().AddCircuitOptions(option => option.DetailedErrors = true);

#endif
        _ = builder.Services
            .AddHttpClient();

        _ = builder.Services
            .AddAuthorizationBuilder()
            .AddPolicy("api", p => p
                    .RequireAuthenticatedUser()
                    .AddAuthenticationSchemes(IdentityConstants.BearerScheme));

        _ = builder.Services.AddValidatorsFromAssemblyContaining<CommandBusSettingsValidator>(ServiceLifetime.Singleton);
        builder.Services.TryAddSingleton<IResiliencyPolicyProvider, ResiliencyPolicyProvider>();
        builder.Services.TryAddScoped<IDomainCommandDispatcher, DependencyInjectionDomainCommandDispatcher>();
        builder.Services.TryAddScoped<IProjectionUpdateProcessor, DependencyInjectionProjectionUpdateProcessor>();
        builder.Services.TryAddSingleton<IDomainAggregateFactory, DomainAggregateFactory>();
        builder.Services
            .TryAddSingleton<IDomainCommandProcessor>((s) => new DomainActorCommandProcessor(
            ActorProxy.DefaultProxyFactory,
            true,
            s.GetRequiredService<ILogger<DomainActorCommandProcessor>>()));

        _ = builder.Services
            .AddDistributedMemoryCache()
            .AddSession(options =>
            {
                options.Cookie.Name = sessionCookieName;
                options.IdleTimeout = TimeSpan.FromMinutes(3);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
        HexalithApplication.AddServerServices(builder.Services, builder.Configuration);
        return builder;
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
            .UseCors()
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

        _ = app
            .UseStaticFiles()
            .UseRouting()
            .UseRequestLocalization()
            .UseAuthentication()
            .UseAuthorization()
            .UseSwagger()
            .UseSwaggerUI()
            .UseSession()
            .UseAntiforgery();
        _ = app.MapControllers();
        _ = app.MapSubscribeHandler();
        _ = app
            .MapRazorComponents<TApp>()
            .AddInteractiveServerRenderMode()
            .AddInteractiveWebAssemblyRenderMode()
            .AddAdditionalAssemblies([.. HexalithApplication.Server.PresentationAssemblies]);

        _ = app.MapActorsHandlers();
        app.UseHexalithModules();

        return app;
    }
}