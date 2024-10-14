// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.WebApis
// Author           : Jérôme Piquot
// Created          : 09-12-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-16-2023
// ***********************************************************************
// <copyright file="HexalithWebApi.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.WebApis.Helpers;

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using Dapr.Actors.Runtime;

using FluentValidation;

using Hexalith.Application;
using Hexalith.Application.Buses;
using Hexalith.Application.Commands;
using Hexalith.Application.Projections;
using Hexalith.Application.Tasks;
using Hexalith.Domain.Messages;
using Hexalith.Extensions.Configuration;
using Hexalith.Infrastructure.AspireService.Defaults;
using Hexalith.Infrastructure.DaprRuntime.Helpers;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

/// <summary>
/// Class HexalithWebApi.
/// </summary>
public static partial class HexalithWebApi
{
    /// <summary>
    /// Creates the application.
    /// </summary>
    /// <param name="applicationName">Name of the application.</param>
    /// <param name="version">The version.</param>
    /// <param name="registerActors">The register actors.</param>
    /// <param name="args">The arguments.</param>
    /// <returns>WebApplicationBuilder.</returns>
    public static WebApplicationBuilder CreateApplication(
        string applicationName,
        string version,
        Action<ActorRegistrationCollection> registerActors,
        string[] args)
    {
        Activity.DefaultIdFormat = ActivityIdFormat.W3C;

        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Serilog.ILogger startupLogger = builder.AddSerilogLogger();

        // startupLogger.Information("Configuring {AppName} ...", applicationName);
        _ = builder.AddServiceDefaults()
            .Services
            .ConfigureSettings<Hexalith.Infrastructure.CosmosDb.Configurations.CosmosDbSettings>(builder.Configuration);

        builder
            .Services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(c =>
            {
                c.SwaggerDoc(
                "v1",
                new()
                {
                    Title = applicationName,
                    Version = version,
                });
                c.OperationFilter<DaprTokenOperationFilter>();
            })
        .AddDaprBuses(builder.Configuration)
        .AddDaprStateStore(builder.Configuration)
        .AddActors(options =>
        {
            // Register actor types and configure actor settings
            registerActors(options.Actors);
            options.UseJsonSerialization = true;
            options.JsonSerializerOptions = ApplicationConstants.DefaultJsonSerializerOptions;
        });
        _ = builder
            .Services
            .AddProblemDetails()
            .AddHttpContextAccessor()
            .AddControllers()
            .AddApplicationPart(typeof(BaseCommand).Assembly) // Issue with MapControllers() throwing a type not found exception for BaseCommand
            .AddApplicationPart(typeof(BaseMessage).Assembly) // Issue with MapControllers() throwing a type not found exception for BaseCommand
            .AddDapr();

        _ = builder.Services.AddAuthentication(); // .AddDapr(); // Adds Dapr authentication
        _ = builder.Services.AddAuthorization(); // options => options.AddDapr());

        // if (debugInVisualStudio)
        // {
        //    // When debugging, we want to be able to run the application inside Visual Studio to see the technical details.
        //    _ = builder.Services.AddDaprSidekick(builder.Configuration);
        // }
        _ = builder.Services.AddValidatorsFromAssemblyContaining<CommandBusSettingsValidator>(ServiceLifetime.Singleton);
        builder.Services.TryAddSingleton(TimeProvider.System);
        builder.Services.TryAddSingleton<IResiliencyPolicyProvider, ResiliencyPolicyProvider>();
        builder.Services.TryAddScoped<IDomainCommandDispatcher, DependencyInjectionDomainCommandDispatcher>();
        builder.Services.TryAddScoped<IProjectionUpdateProcessor, DependencyInjectionProjectionUpdateProcessor>();
        return builder;
    }

    [LoggerMessage(
            LogLevel.Information,
            Message = "Starting application {ApplicationName}")]
    public static partial void LogProgramStartingInformation(ILogger logger, string applicationName);

    /// <summary>
    /// Uses the Hexalith framework.
    /// </summary>
    /// <param name="app">The application.</param>
    /// <param name="applicationName">The application name.</param>
    /// <returns>IApplicationBuilder.</returns>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public static WebApplication UseHexalith<TProgram>([NotNull] this WebApplication app, string applicationName)
    {
        ArgumentNullException.ThrowIfNull(app);
        _ = app
            .UseCors()

            // .UseSerilogRequestLogging()
            .UseCloudEvents();

        _ = app.MapControllers();

        _ = app.MapSubscribeHandler();

        if (app.Environment.IsDevelopment())
        {
            _ = app.UseDeveloperExceptionPage();
        }

        _ = app.UseExceptionHandler();

        _ = app.UseRouting();
        _ = app.UseAuthentication();
        _ = app.UseAuthorization();
        _ = app.MapActorsHandlers();
        _ = app.UseSwagger();
        _ = app.UseSwaggerUI();

        ILogger<TProgram> logger = app
            .Services
            .GetRequiredService<ILogger<TProgram>>();
        LogProgramStartingInformation(logger, applicationName);
        return app;
    }
}