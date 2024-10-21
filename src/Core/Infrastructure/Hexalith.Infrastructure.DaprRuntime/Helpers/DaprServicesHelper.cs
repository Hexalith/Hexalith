// <copyright file="DaprServicesHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Helpers;

using Dapr.Actors;

using Hexalith.Application.Buses;
using Hexalith.Application.Commands;
using Hexalith.Application.Events;

using Hexalith.Application.Requests;
using Hexalith.Application.States;
using Hexalith.Extensions.Configuration;
using Hexalith.Infrastructure.DaprRuntime.Buses;
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

    /// <summary>
    /// Converts a string identifier to an ActorId.
    /// </summary>
    /// <param name="id">The string identifier to convert.</param>
    /// <returns>An <see cref="ActorId"/> created from the escaped string identifier.</returns>
    /// <remarks>
    /// This method escapes the input string to ensure it's a valid ActorId.
    /// </remarks>
    public static ActorId ToActorId(this string id)
        => new(Uri.EscapeDataString(id));

    /// <summary>
    /// Converts an ActorId to its original, unescaped string representation.
    /// </summary>
    /// <param name="id">The ActorId to convert.</param>
    /// <returns>The original, unescaped string representation of the ActorId.</returns>
    /// <remarks>
    /// This method reverses the escaping process performed by ToActorId.
    /// </remarks>
    public static string ToUnescapeString(this ActorId id)
        => Uri.UnescapeDataString(id.ToString());
}