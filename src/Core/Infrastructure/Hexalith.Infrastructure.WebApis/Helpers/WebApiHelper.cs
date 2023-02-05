// <copyright file="WebApiHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.WebApis.Helpers;

using Microsoft.AspNetCore.Builder;

using Serilog;

/// <summary>
/// Class WebApiHelper.
/// </summary>
public static class WebApiHelper
{
    /// <summary>Adds the serilog logger.</summary>
    /// <param name="builder">The builder.</param>
    /// <returns>ILogger.</returns>
    public static ILogger AddSerilogLogger(this WebApplicationBuilder builder)
    {
        ILogger startupLogger = Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateBootstrapLogger();

        _ = builder.Host.UseSerilog(
            (context, services, configuration)
            => configuration
                .ReadFrom.Configuration(context.Configuration)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProcessId()
                .Enrich.WithProcessName()
                .Enrich.WithThreadId()
                .Enrich.WithThreadName()
                .Enrich.WithEnvironmentUserName()
                .WriteTo.Console()
                .ReadFrom.Services(services));
        return startupLogger;
    }

    /// <summary>Uses the serilog logger.</summary>
    /// <param name="app">The application.</param>
    /// <returns>IApplicationBuilder.</returns>
    public static IApplicationBuilder UseSerilogLogger(this IApplicationBuilder app)
    {
        return app.UseSerilogRequestLogging();
    }
}