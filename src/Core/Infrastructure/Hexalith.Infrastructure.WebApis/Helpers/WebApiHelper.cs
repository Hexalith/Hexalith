// <copyright file="WebApiHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.WebApis.Helpers;

using Hexalith.Application.Abstractions.Errors;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;

using Serilog;

using SmartFormat;

using System.Globalization;
using System.Net;

/// <summary>
/// Class WebApiHelper.
/// </summary>
public static class WebApiHelper
{
    /// <summary>
    /// Converts an application error to a Mvc problem details object.
    /// </summary>
    /// <param name="problem">The problem.</param>
    /// <param name="technicalDetail">if set to <c>true</c> [add technical details].</param>
    /// <returns>ProblemDetails.</returns>
    public static ProblemDetails ToProblemDetails(this Error problem, bool technicalDetail)
    {
        string detail = technicalDetail
           ? Smart.Format(
               CultureInfo.InvariantCulture,
               problem.TechnicalDetail ?? string.Empty,
               problem.TechnicalArguments)
            : Smart.Format(
                CultureInfo.InvariantCulture,
                problem.Detail ?? string.Empty,
                problem.Arguments);
        return new()
        {
            Detail = detail,
            Instance = "https://github.com/Hexalith/Hexalith/issues/",
            Status = (int)HttpStatusCode.BadRequest,
            Title = problem.Title,
            Type = problem.Type,
        };
    }

    /// <summary>Adds the serilog logger.</summary>
    /// <param name="builder">The builder.</param>
    /// <returns>ILogger.</returns>
    public static ILogger AddSerilogLogger(this WebApplicationBuilder builder)
    {
        ILogger startupLogger = Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateBootstrapLogger();

        builder.Host.UseSerilog(
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