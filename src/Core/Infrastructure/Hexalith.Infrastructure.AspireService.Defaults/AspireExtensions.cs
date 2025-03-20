// <copyright file="AspireExtensions.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.AspireService.Defaults;

using Azure.Monitor.OpenTelemetry.AspNetCore;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

/// <summary>
/// Provides extension methods for configuring default services in an Aspire application.
/// </summary>
public static class AspireExtensions
{
    /// <summary>
    /// Adds default health checks to the application.
    /// </summary>
    /// <param name="builder">The <see cref="IHostApplicationBuilder"/> to configure.</param>
    /// <returns>The configured <see cref="IHostApplicationBuilder"/>.</returns>
    public static IHostApplicationBuilder AddDefaultHealthChecks(this IHostApplicationBuilder builder)
    {
        _ = builder.Services.AddHealthChecks()

            // Add a default liveness check to ensure app is responsive
            .AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);

        return builder;
    }

    /// <summary>
    /// Adds default services to the Aspire application.
    /// </summary>
    /// <param name="builder">The <see cref="IHostApplicationBuilder"/> to configure.</param>
    /// <returns>The configured <see cref="IHostApplicationBuilder"/>.</returns>
    public static IHostApplicationBuilder AddServiceDefaults(this IHostApplicationBuilder builder)
    {
        _ = builder.ConfigureOpenTelemetry();

        _ = builder.AddDefaultHealthChecks();

        _ = builder.Services.AddServiceDiscovery();

        _ = builder.Services.ConfigureHttpClientDefaults(http =>
        {
            // Turn on resilience by default
            _ = http.AddStandardResilienceHandler();

            // Turn on service discovery by default
            _ = http.AddServiceDiscovery();
        });

        return builder;
    }

    /// <summary>
    /// Configures OpenTelemetry for the application.
    /// </summary>
    /// <param name="builder">The <see cref="IHostApplicationBuilder"/> to configure.</param>
    /// <returns>The configured <see cref="IHostApplicationBuilder"/>.</returns>
    public static IHostApplicationBuilder ConfigureOpenTelemetry(this IHostApplicationBuilder builder)
    {
        // Configure OpenTelemetry logging
        _ = builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.IncludeFormattedMessage = true;
            logging.IncludeScopes = true;
        });

        // Add OpenTelemetry services with metrics and tracing
        _ = builder.Services.AddOpenTelemetry()
            .WithMetrics(metrics => metrics
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddRuntimeInstrumentation())
            .WithTracing(tracing => tracing
                .AddSource(builder.Environment.ApplicationName)
                .AddAspNetCoreInstrumentation()
                .AddGrpcClientInstrumentation()
                .AddHttpClientInstrumentation());

        _ = builder.AddOpenTelemetryExporters();

        return builder;
    }

    /// <summary>
    /// Maps default endpoints for the web application, including health check endpoints in development environment.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> to configure.</param>
    /// <returns>The configured <see cref="WebApplication"/>.</returns>
    /// <remarks>
    /// Adding health checks endpoints to applications in non-development environments has security implications.
    /// See https://aka.ms/dotnet/aspire/healthchecks for details before enabling these endpoints in non-development environments.
    /// </remarks>
    public static WebApplication MapDefaultEndpoints(this WebApplication app)
    {
        // Map health check endpoint for overall application health
        _ = app.MapHealthChecks("/health").AllowAnonymous();

        // Map health check endpoint for liveness probe
        _ = app.MapHealthChecks("/alive", new HealthCheckOptions
        {
            Predicate = r => r.Tags.Contains("live"),
        }).AllowAnonymous();

        return app;
    }

    /// <summary>
    /// Adds OpenTelemetry exporters based on configuration.
    /// </summary>
    /// <param name="builder">The <see cref="IHostApplicationBuilder"/> to configure.</param>
    /// <returns>The configured <see cref="IHostApplicationBuilder"/>.</returns>
    private static IHostApplicationBuilder AddOpenTelemetryExporters(this IHostApplicationBuilder builder)
    {
        bool useOtlpExporter = !string.IsNullOrWhiteSpace(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]);

        if (useOtlpExporter)
        {
            _ = builder
                .Services
                .AddOpenTelemetry()
                .UseOtlpExporter();
        }

        if (!string.IsNullOrEmpty(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]))
        {
            _ = builder.Services
                .AddOpenTelemetry()
                .UseAzureMonitor();
        }

        return builder;
    }
}