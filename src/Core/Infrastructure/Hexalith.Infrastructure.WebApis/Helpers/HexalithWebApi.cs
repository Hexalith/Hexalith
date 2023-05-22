// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.WebApis
// Author           : jpiquot
// Created          : 01-15-2023
//
// Last Modified By : jpiquot
// Last Modified On : 02-18-2023
// ***********************************************************************
// <copyright file="HexalithWebApi.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.WebApis.Helpers;

using System.Diagnostics;

using Dapr.Actors.Runtime;

using FluentValidation;

using Hexalith.Application.Aggregates;
using Hexalith.Application.Buses;
using Hexalith.Application.Commands;
using Hexalith.Application.Events;
using Hexalith.Application.Notifications;
using Hexalith.Application.Requests;
using Hexalith.Application.States;
using Hexalith.Domain.Messages;
using Hexalith.Extensions.Common;
using Hexalith.Extensions.Configuration;
using Hexalith.Infrastructure.DaprRuntime.Buses;
using Hexalith.Infrastructure.DaprRuntime.Helpers;
using Hexalith.Infrastructure.DaprRuntime.States;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;

/// <summary>
/// Class HexalithWebApi.
/// </summary>
public static class HexalithWebApi
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

        builder.Services.AddDaprClient();

        builder.Services.AddActors(options =>

            // Register actor types and configure actor settings
            registerActors(options.Actors));
        _ = builder
            .Services
            .AddControllers()
            .AddApplicationPart(typeof(BaseMessage).Assembly) // Issue with MapControllers() throwing a type not found exception for BaseCommand
            .AddDapr();

        if (debugInVisualStudio == true)
        {
            // When debugging, we want to be able to run the application inside Visual Studio to see the technical details.
            _ = builder.Services.AddDaprSidekick(builder.Configuration);
        }

        _ = builder.Services.AddValidatorsFromAssemblyContaining<CommandBusSettingsValidator>(ServiceLifetime.Singleton);
        _ = builder.Services.AddDaprHandlers(builder.Configuration);
        _ = builder.Services.AddSingleton<IDateTimeService, DateTimeService>();
        _ = builder.Services.ConfigureSettings<EventBusSettings>(builder.Configuration);
        _ = builder.Services.ConfigureSettings<CommandBusSettings>(builder.Configuration);
        _ = builder.Services.ConfigureSettings<NotificationBusSettings>(builder.Configuration);
        _ = builder.Services.ConfigureSettings<RequestBusSettings>(builder.Configuration);
        _ = builder.Services.ConfigureSettings<StateStoreSettings>(builder.Configuration);
        _ = builder.Services.AddSingleton<IRequestBus, DaprRequestBus>();
        _ = builder.Services.AddSingleton<INotificationBus, DaprNotificationBus>();
        _ = builder.Services.AddSingleton<ICommandBus, DaprCommandBus>();
        _ = builder.Services.AddSingleton<IEventBus, DaprEventBus>();
        _ = builder.Services.AddSingleton<IAggregateStateManager, AggregateStateManager>();
        _ = builder.Services.AddSingleton<IStateStoreProvider, DaprClientStateStoreProvider>();
        _ = builder.Services.AddSingleton<ICommandDispatcher, DependencyInjectionCommandDispatcher>();

        return builder;
    }

    /// <summary>
    /// Uses the Hexalith framework.
    /// </summary>
    /// <param name="app">The application.</param>
    /// <returns>IApplicationBuilder.</returns>
    public static IApplicationBuilder UseHexalith(this WebApplication app)
    {
        _ = app.UseSerilogRequestLogging();

        _ = app.UseCloudEvents();

        _ = app.MapControllers();

        _ = app.MapSubscribeHandler();

        if (app.Environment.IsDevelopment())
        {
            _ = app.UseDeveloperExceptionPage();
            _ = app.UseSwagger();
            _ = app.UseSwaggerUI();
        }

        _ = app.UseRouting();

        _ = app.MapActorsHandlers();
        return app;
    }
}