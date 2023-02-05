// <copyright file="HexalithWebApi.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.WebApis.Helpers;

using System.Diagnostics;
using System.Text.Json;

using Dapr.Actors.Runtime;

using Hexalith.Application.Abstractions.Aggregates;
using Hexalith.Application.Abstractions.Commands;
using Hexalith.Application.Abstractions.Events;
using Hexalith.Application.Abstractions.Notifications;
using Hexalith.Application.Abstractions.Requests;
using Hexalith.Application.Commands;
using Hexalith.Application.States;
using Hexalith.Extensions.Common;
using Hexalith.Extensions.Configuration;
using Hexalith.Infrastructure.DaprBus;
using Hexalith.Infrastructure.DaprBus.Configuration;
using Hexalith.Infrastructure.DaprHandlers.Helpers;
using Hexalith.Infrastructure.Serialization;
using Hexalith.Infrastructure.Serialization.Helpers;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Class HexalithWebApi.
/// </summary>
public static class HexalithWebApi
{
    /// <summary>Creates the hexalith application.</summary>
    /// <param name="applicationName">Name of the application.</param>
    /// <param name="version">The API version, for example 'v1'.</param>
    /// <param name="debugInVisualStudio">If true, runs the application inside Visual Studio to simplify debugging.</param>
    /// <param name="registerActors">Used to register application actors.</param>
    /// <param name="args">The program arguments.</param>
    /// <returns>WebApplicationBuilder.</returns>
    public static WebApplicationBuilder CreateApplication(
        string applicationName,
        string version,
        bool debugInVisualStudio,
        Action<ActorRegistrationCollection> registerActors,
        string[] args)
    {
        Activity.DefaultIdFormat = ActivityIdFormat.W3C;

        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        Serilog.ILogger startupLogger = builder.AddSerilogLogger();

        startupLogger.Information("Configuring {AppName} ...", applicationName);
        _ = builder.Services.AddEndpointsApiExplorer();

        _ = builder.Services.AddSwaggerGen(c => c.SwaggerDoc("v1", new() { Title = applicationName, Version = version, }));

        _ = builder.Services.AddControllers().AddDapr();

        if (debugInVisualStudio == true)
        {
            // When debugging, we want to be able to run the application inside Visual Studio to see the technical details.
            _ = builder.Services.AddDaprSidekick(builder.Configuration);
        }

        builder.Services.AddDaprClient(configure => configure.UseJsonSerializationOptions(new JsonSerializerOptions().AddPolymorphism()));
        builder.Services.AddActors(options =>
        {
            // Register actor types and configure actor settings
            registerActors(options.Actors);

            // Configure serialization options
            options.JsonSerializerOptions.TypeInfoResolver = new PolymorphicTypeResolver();
        });
        _ = builder.Services.AddDaprHandlers(builder.Configuration);
        _ = builder.Services.AddSingleton<IDateTimeService, DateTimeService>();
        _ = builder.Services.ConfigureSettings<DaprEventBusSettings>(builder.Configuration);
        _ = builder.Services.ConfigureSettings<DaprCommandBusSettings>(builder.Configuration);
        _ = builder.Services.ConfigureSettings<DaprNotificationBusSettings>(builder.Configuration);
        _ = builder.Services.ConfigureSettings<DaprRequestBusSettings>(builder.Configuration);
        _ = builder.Services.AddSingleton<IRequestBus, DaprRequestBus>();
        _ = builder.Services.AddSingleton<INotificationBus, DaprNotificationBus>();
        _ = builder.Services.AddSingleton<ICommandBus, DaprCommandBus>();
        _ = builder.Services.AddSingleton<IEventBus, DaprEventBus>();
        _ = builder.Services.AddSingleton<IAggregateStateManager, AggregateStateManager>();
        _ = builder.Services.AddSingleton<ICommandDispatcher, DependencyInjectionCommandDispatcher>();

        return builder;
    }
}