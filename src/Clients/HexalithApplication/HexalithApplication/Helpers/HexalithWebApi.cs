// <copyright file="HexalithWebApi.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

using Hexalith.Infrastructure.WebApis.Helpers;

namespace HexalithApplication.Helpers;

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using Dapr.Actors.Runtime;

using FluentValidation;

using Hexalith.Application.Aggregates;
using Hexalith.Application.Buses;
using Hexalith.Application.Commands;
using Hexalith.Application.Projection;
using Hexalith.Application.States;
using Hexalith.Application.Tasks;
using Hexalith.Domain.Messages;
using Hexalith.Extensions.Common;
using Hexalith.Infrastructure.DaprRuntime.Helpers;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

using Serilog;

/// <summary>
/// Class HexalithWebApi.
/// </summary>
public static class HexalithWebApplicationHelper
{
    /// <summary>
    /// Creates the Hexalith application.
    /// </summary>
    /// <param name="applicationName">Name of the application.</param>
    /// <param name="version">The API version, for example 'v1'.</param>
    /// <param name="debugInVisualStudio">If true, runs the application inside Visual Studio to simplify debugging.</param>
    /// <param name="registerActors">Used to register application actors.</param>
    /// <param name="args">The program arguments.</param>
    /// <returns>WebApplicationBuilder.</returns>
    public static WebApplicationBuilder CreateWebApplication(
        string applicationName,
        string version,
        bool debugInVisualStudio,
        Action<ActorRegistrationCollection> registerActors,
        string[] args)
    {
        Activity.DefaultIdFormat = ActivityIdFormat.W3C;

        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        ILogger startupLogger = builder.AddSerilogLogger();

        startupLogger.Information("Configuring {AppName} ...", applicationName);
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

        if (debugInVisualStudio == true)
        {
            // When debugging, we want to be able to run the application inside Visual Studio to see the technical details.
            _ = builder.Services.AddDaprSidekick(builder.Configuration);
        }

        _ = builder.Services.AddValidatorsFromAssemblyContaining<CommandBusSettingsValidator>(ServiceLifetime.Singleton);
        builder.Services.TryAddSingleton<IDateTimeService, DateTimeService>();
        builder.Services.TryAddSingleton<IResiliencyPolicyProvider, ResiliencyPolicyProvider>();
        builder.Services.TryAddScoped<IAggregateStateManager, AggregateStateManager>();
        builder.Services.TryAddScoped<ICommandDispatcher, DependencyInjectionCommandDispatcher>();
        builder.Services.TryAddScoped<IProjectionUpdateProcessor, DependencyInjectionProjectionUpdateProcessor>();

        return builder;
    }

    /// <summary>
    /// Uses the Hexalith framework.
    /// </summary>
    /// <param name="app">The application.</param>
    /// <returns>IApplicationBuilder.</returns>
    /// <exception cref="ArgumentNullException">null.</exception>
    public static IApplicationBuilder UseHexalith([NotNull] this WebApplication app)
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
            _ = app.UseDeveloperExceptionPage();
        }

        _ = app.UseSwagger();
        _ = app.UseSwaggerUI();

        _ = app.UseRouting();

        _ = app.MapActorsHandlers();
        return app;
    }
}