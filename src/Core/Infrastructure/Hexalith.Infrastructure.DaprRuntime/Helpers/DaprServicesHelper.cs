// <copyright file="DaprServicesHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Helpers;

using Hexalith.Application.Buses;
using Hexalith.Application.Commands;
using Hexalith.Application.Events;

using Hexalith.Application.Requests;
using Hexalith.Application.Services;
using Hexalith.Application.States;
using Hexalith.Applications.Commands;
using Hexalith.Extensions.Configuration;
using Hexalith.Infrastructure.DaprRuntime.Buses;
using Hexalith.Infrastructure.DaprRuntime.Services;
using Hexalith.Infrastructure.DaprRuntime.States;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// Provides helper methods for configuring Dapr-related services in the dependency injection container.
/// This class facilitates the setup of Dapr buses, state stores, and actor-related utilities.
/// </summary>
public static class DaprServicesHelper
{
    /// <summary>
    /// Adds the Dapr Aggregate Services to the service collection.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the Aggregate Services to.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddDaprAggregateServices(this IServiceCollection services)
    {
        _ = services.AddTransient<IAggregateService, AggregateService>();
        return services;
    }

    /// <summary>
    /// Adds all Dapr buses (Command, Event, and Request) to the service collection.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <param name="configuration">The configuration containing settings for the buses.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    /// <remarks>
    /// This method is a convenience wrapper that calls AddDaprEventBus, AddDaprCommandBus, and AddDaprRequestBus.
    /// </remarks>
    public static IServiceCollection AddDaprBuses(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddDaprEventBus(configuration);
        _ = services.AddDaprCommandBus(configuration);
        _ = services.AddDaprRequestBus(configuration);
        return services;
    }

    /// <summary>
    /// Adds the Dapr Command Bus to the service collection.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the Command Bus to.</param>
    /// <param name="configuration">The configuration containing Command Bus settings.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    /// <remarks>
    /// This method configures the CommandBusSettings and registers the DaprCommandBus as the implementation for ICommandBus.
    /// </remarks>
    public static IServiceCollection AddDaprCommandBus(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.ConfigureSettings<CommandBusSettings>(configuration);
        services.TryAddSingleton<ICommandBus, DaprCommandBus>();
        return services;
    }

    /// <summary>
    /// Adds the Dapr Event Bus to the service collection.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the Event Bus to.</param>
    /// <param name="configuration">The configuration containing Event Bus settings.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    /// <remarks>
    /// This method configures the EventBusSettings and registers the DaprEventBus as the implementation for IEventBus.
    /// </remarks>
    public static IServiceCollection AddDaprEventBus(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.ConfigureSettings<EventBusSettings>(configuration);
        services.TryAddSingleton<IEventBus, DaprEventBus>();
        return services;
    }

    /// <summary>
    /// Adds the Dapr Request Bus to the service collection.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the Request Bus to.</param>
    /// <param name="configuration">The configuration containing Request Bus settings.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    /// <remarks>
    /// This method configures the RequestBusSettings and registers the DaprRequestBus as the implementation for IRequestBus.
    /// </remarks>
    public static IServiceCollection AddDaprRequestBus(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.ConfigureSettings<RequestBusSettings>(configuration);
        services.TryAddSingleton<IRequestBus, DaprRequestBus>();
        return services;
    }

    /// <summary>
    /// Adds the Dapr State Store to the service collection.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the State Store to.</param>
    /// <param name="configuration">The configuration containing State Store settings.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    /// <remarks>
    /// This method configures the StateStoreSettings and registers the DaprClientStateStoreProvider as the implementation for IStateStoreProvider.
    /// </remarks>
    public static IServiceCollection AddDaprStateStore(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.ConfigureSettings<StateStoreSettings>(configuration);
        services.TryAddScoped<IStateStoreProvider, DaprClientStateStoreProvider>();
        return services;
    }
}