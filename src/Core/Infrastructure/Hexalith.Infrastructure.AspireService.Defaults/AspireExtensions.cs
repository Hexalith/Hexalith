// <copyright file="AspireExtensions.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.AspireService.Defaults;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

/// <summary>
/// Provides extension methods for configuring default services in an Aspire application.
/// </summary>
public static class AspireExtensions
{
    /// <summary>
    /// Adds default services to the application.
    /// </summary>
    /// <param name="builder">The <see cref="IHostApplicationBuilder"/> instance.</param>
    /// <returns>The <see cref="IHostApplicationBuilder"/>builder instance.</returns>
    public static IHostApplicationBuilder AddServiceDefaults(this IHostApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        _ = builder.ConfigureOpenTelemetry();

        return builder;
    }

    /// <summary>
    /// Configures OpenTelemetry for the application.
    /// </summary>
    /// <param name="builder">The <see cref="IHostApplicationBuilder"/> instance.</param>
    /// <returns>The <see cref="IHostApplicationBuilder"/>builder instance.</returns>
    public static IHostApplicationBuilder ConfigureOpenTelemetry(this IHostApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        _ = builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.IncludeFormattedMessage = true;
            logging.IncludeScopes = true;
        });

        _ = builder.Services.AddOpenTelemetry()
           .WithMetrics(metrics => _ = metrics.AddRuntimeInstrumentation()
                      .AddBuiltInMeters())
           .WithTracing(tracing =>
           {
               if (builder.Environment.IsDevelopment())
               {
                   // We want to view all traces in development
                   _ = tracing.SetSampler(new AlwaysOnSampler());
               }

               _ = tracing.AddAspNetCoreInstrumentation()
                      .AddGrpcClientInstrumentation()
                      .AddHttpClientInstrumentation();
           });

        _ = builder.AddOpenTelemetryExporters();

        return builder;
    }

    private static MeterProviderBuilder AddBuiltInMeters(this MeterProviderBuilder meterProviderBuilder) =>
         meterProviderBuilder.AddMeter(
             "Microsoft.AspNetCore.Hosting",
             "Microsoft.AspNetCore.Server.Kestrel",
             "System.Net.Http");

    private static IHostApplicationBuilder AddOpenTelemetryExporters(this IHostApplicationBuilder builder)
    {
        bool useOtlpExporter = !string.IsNullOrWhiteSpace(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]);

        if (useOtlpExporter)
        {
            _ = builder.Services.Configure<OpenTelemetryLoggerOptions>(logging => logging.AddOtlpExporter());
            _ = builder.Services.ConfigureOpenTelemetryMeterProvider(metrics => metrics.AddOtlpExporter());
            _ = builder.Services.ConfigureOpenTelemetryTracerProvider(tracing => tracing.AddOtlpExporter());
        }

        return builder;
    }
}