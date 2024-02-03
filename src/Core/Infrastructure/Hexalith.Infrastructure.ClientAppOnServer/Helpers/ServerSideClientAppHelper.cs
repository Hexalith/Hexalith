// ***********************************************************************
// Assembly         : HexalithApplication
// Author           : Jérôme Piquot
// Created          : 01-18-2024
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-18-2024
// ***********************************************************************
// <copyright file="ServerSideClientAppHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.ClientAppOnServer.Helpers;

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using Dapr.Actors.Runtime;

using Hexalith.Application.Aggregates;
using Hexalith.Application.Buses;
using Hexalith.Application.Commands;
using Hexalith.Application.Projection;
using Hexalith.Application.States;
using Hexalith.Application.Tasks;
using Hexalith.Domain.Messages;
using Hexalith.Extensions.Common;
using Hexalith.Infrastructure.ClientApp.Helpers;
using Hexalith.Infrastructure.DaprRuntime.Helpers;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Class HexalithWebApi.
/// </summary>
public static class ServerSideClientAppHelper
{
    public static IServiceCollection AddHexalithServerSideClientApp(this IServiceCollection services, IConfiguration configuration)
            => services.AddHexalithClientApp(configuration);

    /// <summary>
    /// Creates the web application.
    /// </summary>
    /// <param name="applicationName">Name of the application.</param>
    /// <param name="sessionCookieName">Name of the session cookie.</param>
    /// <param name="version">The version.</param>
    /// <param name="debugInVisualStudio">if set to <c>true</c> [debug in visual studio].</param>
    /// <param name="registerActors">The register actors.</param>
    /// <param name="args">The arguments.</param>
    /// <returns>WebApplicationBuilder.</returns>
    /// <exception cref="InvalidOperationException">Connection string 'DefaultConnection' not found.</exception>
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
        ILogger startupLogger = builder.AddSerilogLogger();

        startupLogger.LogInformation("Configuring {AppName} ...", applicationName);
        builder.Services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(c => c.SwaggerDoc("v1", new() { Title = applicationName, Version = version, }))
            .AddDaprBuses(builder.Configuration)
            .AddDaprStateStore(builder.Configuration)
            .AddActors(options =>

            // Register actor types and configure actor settings
            registerActors(options.Actors));
        _ = builder
            .Services
            .AddHttpContextAccessor()
            .AddControllers()
            .AddApplicationPart(typeof(BaseCommand).Assembly) // Issue with MapControllers() throwing a type not found exception for BaseCommand
            .AddApplicationPart(typeof(BaseMessage).Assembly) // Issue with MapControllers() throwing a type not found exception for BaseCommand
            .AddDapr();

        // Add email services for sending authentication emails.
        _ = builder.Services
            .AddHexalithServerSideClientApp(builder.Configuration)
            .AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddInteractiveWebAssemblyComponents();

        _ = builder.Services
            .AddHttpClient()
            .AddFluentUIComponents()
            .AddCascadingAuthenticationState()
            .AddScoped<IdentityUserAccessor>()
            .AddScoped<IdentityRedirectManager>()
            .AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>()
            .AddSingleton<IEmailSender<ApplicationUser>, IdentityEmailSender>()
            .AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddBearerToken(IdentityConstants.BearerScheme)
            .AddMicrosoftAccount(microsoftOptions =>
            {
                string? clientId = builder.Configuration["Authentication:Microsoft:ClientId"];
                string? clientSecret = builder.Configuration["Authentication:Microsoft:ClientSecret"];
                if (string.IsNullOrWhiteSpace(clientId))
                {
                    throw new InvalidOperationException("Authentication:Microsoft:ClientId must be set in the application settings.");
                }

                if (string.IsNullOrWhiteSpace(clientSecret))
                {
                    throw new InvalidOperationException("Authentication:Microsoft:ClientSecret must be set in the application settings.");
                }

                microsoftOptions.ClientId = clientId;
                microsoftOptions.ClientSecret = clientSecret;
            })
            .AddIdentityCookies();

        _ = builder.Services
            .AddAuthorizationBuilder()
            .AddPolicy("api", p => _ = p
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
            // When debugging, we want to be able to run the application inside Visual Studio to see the technical details.
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

        return builder;
    }

    /// <summary>
    /// Uses the Hexalith framework.
    /// </summary>
    /// <param name="app">The application.</param>
    /// <returns>IApplicationBuilder.</returns>
    /// <exception cref="ArgumentNullException">null.</exception>
    public static IApplicationBuilder UseHexalithWebApplication([NotNull] this WebApplication app)
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
            .MapRazorComponents<App>()
            .AddInteractiveServerRenderMode()
            .AddInteractiveWebAssemblyRenderMode()
            .AddAdditionalAssemblies(typeof(HexalithApplication.Client._Imports).Assembly);

        // Add additional endpoints required by the Identity /Account Razor components.
        _ = app.MapAdditionalIdentityEndpoints();

        // _ = app.MapGroup("api/auth").MapIdentityApi<ApplicationUser>();
        _ = app.MapActorsHandlers();

        return app;
    }
}