// <copyright file="ServerSideClientAppHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.ClientAppOnServer.Helpers;

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using Dapr.Actors.Runtime;

using FluentValidation;

using Hexalith.Application.Aggregates;
using Hexalith.Application.Buses;
using Hexalith.Application.Commands;
using Hexalith.Application.Projection;
using Hexalith.Application.States;
using Hexalith.Application.Tasks;
using Hexalith.Domain.Messages;
using Hexalith.Infrastructure.ClientApp.Helpers;
using Hexalith.Infrastructure.ClientAppOnServer.Security;
using Hexalith.Infrastructure.DaprRuntime.Helpers;
using Hexalith.Infrastructure.GoogleMaps.Helpers;
using Hexalith.Infrastructure.Security.Abstractions.Models;
using Hexalith.Infrastructure.WebApis.Helpers;
using Hexalith.UI.Authentications.Helpers;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;

using Serilog;

/// <summary>
/// Class HexalithWebApi.
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
        IConfiguration configuration) => services.AddHexalithClientApp(configuration);

    /// <summary>
    /// Creates the server-side client application.
    /// </summary>
    /// <param name="applicationName">The name of the application.</param>
    /// <param name="sessionCookieName">The name of the session cookie.</param>
    /// <param name="version">The version of the application.</param>
    /// <param name="debugInVisualStudio">Indicates whether the application is being debugged in Visual Studio.</param>
    /// <param name="registerActors">The action to register actors.</param>
    /// <param name="args">The command-line arguments.</param>
    /// <returns>The web application builder.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the connection string 'DefaultConnection' is not found.</exception>
    public static WebApplicationBuilder CreateServerSideClientApplication(
        string applicationName,
        string sessionCookieName,
        string version,
        bool debugInVisualStudio,
        Action<ActorRegistrationCollection> registerActors,
        string[] args)
    {
        Activity.DefaultIdFormat = ActivityIdFormat.W3C;

        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        Serilog.ILogger startupLogger = builder.AddSerilogLogger();

        startupLogger.Information("Configuring {AppName} ...", applicationName);
        builder.Services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(c => c.SwaggerDoc("v1", new() { Title = applicationName, Version = version, }))
            .AddDaprBuses(builder.Configuration)
            .AddDaprStateStore(builder.Configuration)
            .AddActors(options => registerActors(options.Actors));
        _ = builder
            .Services
            .AddHttpContextAccessor()
            .AddControllers()
            .AddApplicationPart(typeof(BaseCommand).Assembly)
            .AddApplicationPart(typeof(BaseMessage).Assembly)
            .AddDapr();

        _ = builder.Services
            .AddHexalithServerSideClientApp(builder.Configuration)
            .AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddInteractiveWebAssemblyComponents();

        _ = builder.Services
            .AddHttpClient()
            .AddFluentUIComponents();

        _ = builder.Services
            .AddAuthorizationBuilder()
            .AddPolicy("api", p => p
                    .RequireAuthenticatedUser()
                    .AddAuthenticationSchemes(IdentityConstants.BearerScheme));

        string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        _ = builder.Services
            .AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString))
            .AddDatabaseDeveloperPageExceptionFilter()
            .AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders()
            .AddApiEndpoints();

        if (debugInVisualStudio)
        {
            _ = builder.Services.AddDaprSidekick(builder.Configuration);
        }

        _ = builder.Services.AddValidatorsFromAssemblyContaining<CommandBusSettingsValidator>(ServiceLifetime.Singleton);
        builder.Services.TryAddSingleton<IResiliencyPolicyProvider, ResiliencyPolicyProvider>();
        builder.Services.TryAddScoped<IAggregateStateManager, AggregateStateManager>();
        builder.Services.TryAddScoped<ICommandDispatcher, DependencyInjectionCommandDispatcher>();
        builder.Services.TryAddScoped<IProjectionUpdateProcessor, DependencyInjectionProjectionUpdateProcessor>();

        _ = builder.Services
            .AddDistributedMemoryCache()
            .AddSession(options =>
            {
                options.Cookie.Name = sessionCookieName;
                options.IdleTimeout = TimeSpan.FromMinutes(3);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
        _ = builder.Services
            .AddAuthenticationUI(builder.Configuration)
            .AddGeolocationServices()
            .AddGooglePlacesServices(builder.Configuration);
        return builder;
    }

    /// <summary>
    /// Uses the Hexalith framework for the server-side client application.
    /// </summary>
    /// <typeparam name="TApp">The type of the application.</typeparam>
    /// <param name="app">The web application.</param>
    /// <param name="additionalAssemblies">The assembly containing the WebAssembly client.</param>
    /// <returns>The updated web application.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the web application is null.</exception>
    public static IApplicationBuilder UseHexalithWebApplication<TApp, TUser>([NotNull] this WebApplication app, Assembly[] additionalAssemblies)
        where TUser : class, new()
    {
        ArgumentNullException.ThrowIfNull(app);
        _ = app
            .UseCors()
            .UseSerilogRequestLogging()
            .UseCloudEvents();

        _ = app.MapControllers();

        _ = app.MapSubscribeHandler();

        if (app.Environment.IsDevelopment())
        {
            app
                .UseDeveloperExceptionPage()
                .UseMigrationsEndPoint()
                .UseWebAssemblyDebugging();
        }

        _ = app
            .UseStaticFiles()
            .UseSwagger()
            .UseSwaggerUI()
            .UseRouting()
            .UseAuthentication()
            .UseAuthorization()
            .UseSession()
            .UseAntiforgery()
            .UseRequestLocalization();

        _ = app
            .MapRazorComponents<TApp>()
            .AddInteractiveServerRenderMode()
            .AddInteractiveWebAssemblyRenderMode()
            .AddAdditionalAssemblies(additionalAssemblies);

        _ = app.MapIdentityApi<TUser>();

        _ = app.MapActorsHandlers();

        return app;
    }
}