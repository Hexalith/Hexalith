// <copyright file="HexalithWebApi.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.WebApis.Helpers;

using Dapr.Actors.Runtime;

using Hexalith.Extensions.Common;
using Hexalith.Infrastructure.DaprHandlers.Helpers;
using Hexalith.Infrastructure.Serialization;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using System.Diagnostics;

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
        var startupLogger = builder.AddSerilogLogger();

        startupLogger.Information("Configuring {AppName} ...", applicationName);
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(c => c.SwaggerDoc("v1", new() { Title = applicationName, Version = version, }));

        builder.Services.AddControllers().AddDapr();

        if (debugInVisualStudio == true)
        {
            // When debugging, we want to be able to run the application inside Visual Studio to see the technical details.
            builder.Services.AddDaprSidekick(builder.Configuration);
        }

        builder.Services.AddDaprClient();
        builder.Services.AddActors(options =>
        {
            // Register actor types and configure actor settings
            registerActors(options.Actors);

            // Configure serialization options
            options.JsonSerializerOptions.TypeInfoResolver = new PolymorphicTypeResolver();
        });
        builder.Services.AddDaprHandlers(builder.Configuration);
        builder.Services.AddSingleton<IDateTimeService, DateTimeService>();
        return builder;
    }
}