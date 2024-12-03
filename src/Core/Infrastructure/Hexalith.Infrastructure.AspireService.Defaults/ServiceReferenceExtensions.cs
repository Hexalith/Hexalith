// <copyright file="ServiceReferenceExtensions.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.AspireService.Defaults;

using Grpc.Health.V1;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

/// <summary>
/// Provides extension methods for adding service references to the <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceReferenceExtensions
{
    /// <summary>
    /// Adds a gRPC service reference. Configures a binding between the <typeparamref name="TClient"/> type and a named <see cref="HttpClient"/>
    /// with an address. The client name will be set to the type name of <typeparamref name="TClient"/>.
    /// </summary>
    /// <typeparam name="TClient">The type of the gRPC client. The type specified will be registered in the service collection as a transient service.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="address">
    /// The value to assign to the <see cref="Grpc.Net.ClientFactory.GrpcClientFactoryOptions.Address"/> property of the typed gRPC client's injected
    /// <see cref="Grpc.Net.ClientFactory.GrpcClientFactoryOptions"/> instance.
    /// </param>
    /// <returns>An <see cref="IHttpClientBuilder"/> that can be used to configure the client.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="address"/> is not a valid URI value.</exception>
    public static IHttpClientBuilder AddGrpcServiceReference<TClient>(this IServiceCollection services, string address)
        where TClient : class
    {
        ArgumentNullException.ThrowIfNull(services);

        if (!Uri.IsWellFormedUriString(address, UriKind.Absolute))
        {
            throw new ArgumentException("Address must be a valid absolute URI.", nameof(address));
        }

        IHttpClientBuilder builder = services.AddGrpcClient<TClient>(o => o.Address = new(address));

        return builder;
    }

    /// <summary>
    /// Adds a gRPC service reference. Configures a binding between the <typeparamref name="TClient"/> type and a named <see cref="HttpClient"/>
    /// with an address and gRPC-based health check. The client name will be set to the type name of <typeparamref name="TClient"/>.
    /// </summary>
    /// <remarks>
    /// Note that the gRPC service must be configured to use gRPC health checks. See https://learn.microsoft.com/aspnet/core/grpc/health-checks for more details.
    /// </remarks>
    /// <typeparam name="TClient">The type of the gRPC client. The type specified will be registered in the service collection as a transient service.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="address">
    /// The value to assign to the <see cref="Grpc.Net.ClientFactory.GrpcClientFactoryOptions.Address"/> property of the typed gRPC client's injected
    /// <see cref="Grpc.Net.ClientFactory.GrpcClientFactoryOptions"/> instance.
    /// </param>
    /// <param name="healthCheckName">The health check name.</param>
    /// <param name="failureStatus">The <see cref="HealthStatus"/> that should be reported if the health check fails. Defaults to <see cref="HealthStatus.Unhealthy"/>.</param>
    /// <returns>An <see cref="IHttpClientBuilder"/> that can be used to configure the client.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="address"/> is not a valid URI value.</exception>
    public static IHttpClientBuilder AddGrpcServiceReference<TClient>(this IServiceCollection services, string address, string? healthCheckName, HealthStatus failureStatus)
        where TClient : class
    {
        ArgumentNullException.ThrowIfNull(services);

        if (!Uri.IsWellFormedUriString(address, UriKind.Absolute))
        {
            throw new ArgumentException("Address must be a valid absolute URI.", nameof(address));
        }

        Uri uri = new(address);
        IHttpClientBuilder builder = services.AddGrpcClient<TClient>(o => o.Address = uri);

        AddGrpcHealthChecks(services, uri, healthCheckName ?? $"{typeof(TClient).Name}-health", failureStatus);

        return builder;
    }

    private static void AddGrpcHealthChecks(IServiceCollection services, Uri uri, string healthCheckName, HealthStatus failureStatus = default)
    {
        _ = services.AddGrpcClient<Health.HealthClient>(o => o.Address = uri);
        _ = services.AddHealthChecks()
            .AddCheck<GrpcServiceHealthCheck>(healthCheckName, failureStatus);
    }

    private sealed class GrpcServiceHealthCheck(Health.HealthClient healthClient) : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            HealthCheckResponse response = await healthClient.CheckAsync(new(), cancellationToken: cancellationToken);

            return response.Status switch
            {
                HealthCheckResponse.Types.ServingStatus.Serving => HealthCheckResult.Healthy(),
                HealthCheckResponse.Types.ServingStatus.Unknown => throw new NotSupportedException(),
                HealthCheckResponse.Types.ServingStatus.NotServing => throw new NotSupportedException(),
                HealthCheckResponse.Types.ServingStatus.ServiceUnknown => throw new NotSupportedException(),
                _ => HealthCheckResult.Unhealthy(),
            };
        }
    }
}