// <copyright file="WebApiHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.WebApis.Helpers;

using System.Diagnostics.CodeAnalysis;
using System.Globalization;

using Microsoft.AspNetCore.Builder;

using Serilog;

/// <summary>
/// Class WebApiHelper.
/// </summary>
public static class WebApiHelper
{
    /// <summary>
    /// Adds the Serilog logger.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns>ILogger.</returns>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public static ILogger AddSerilogLogger([NotNull] this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ILogger startupLogger = Log.Logger = new LoggerConfiguration()
            .Enrich
                .FromLogContext()
            .WriteTo
                .Console(formatProvider: CultureInfo.InvariantCulture)
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
                .WriteTo.Console(formatProvider: CultureInfo.InvariantCulture)
                .ReadFrom.Services(services));
        return startupLogger;
    }

    /// <summary>
    /// Uses the serilog logger.
    /// </summary>
    /// <param name="app">The application.</param>
    /// <returns>IApplicationBuilder.</returns>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public static IApplicationBuilder UseSerilogLogger([NotNull] this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);
        return app.UseSerilogRequestLogging();
    }
}