// <copyright file="DaprServicesHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Helpers;

using Hexalith.Application.Buses;
using Hexalith.Application.Commands;
using Hexalith.Application.Events;
using Hexalith.Application.Notifications;
using Hexalith.Application.Requests;
using Hexalith.Application.States;
using Hexalith.Extensions.Configuration;
using Hexalith.Infrastructure.DaprRuntime.Buses;
using Hexalith.Infrastructure.DaprRuntime.States;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// Class DaprServicesHelper.
/// </summary>
public static class DaprServicesHelper
{
    /// <summary>
    /// Adds the dapr buses.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>IServiceCollection.</returns>
    public static IServiceCollection AddDaprBuses(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddDaprEventBus(configuration);
        _ = services.AddDaprCommandBus(configuration);
        _ = services.AddDaprNotificationBus(configuration);
        _ = services.AddDaprRequestBus(configuration);
        return services;
    }

    /// <summary>
    /// Adds the dapr command bus.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>IServiceCollection.</returns>
    public static IServiceCollection AddDaprCommandBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDaprClient();
        _ = services.ConfigureSettings<CommandBusSettings>(configuration);
        services.TryAddSingleton<ICommandBus, DaprCommandBus>();
        return services;
    }

    /// <summary>
    /// Adds the dapr event bus.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>IServiceCollection.</returns>
    public static IServiceCollection AddDaprEventBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDaprClient();
        _ = services.ConfigureSettings<EventBusSettings>(configuration);
        services.TryAddSingleton<IEventBus, DaprEventBus>();
        return services;
    }

    /// <summary>
    /// Adds the dapr notification bus.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>IServiceCollection.</returns>
    public static IServiceCollection AddDaprNotificationBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDaprClient();
        _ = services.ConfigureSettings<NotificationBusSettings>(configuration);
        services.TryAddSingleton<INotificationBus, DaprNotificationBus>();
        return services;
    }

    /// <summary>
    /// Adds the dapr request bus.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>IServiceCollection.</returns>
    public static IServiceCollection AddDaprRequestBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDaprClient();
        _ = services.ConfigureSettings<RequestBusSettings>(configuration);
        services.TryAddSingleton<IRequestBus, DaprRequestBus>();
        return services;
    }

    /// <summary>
    /// Adds the dapr state store.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>IServiceCollection.</returns>
    public static IServiceCollection AddDaprStateStore(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDaprClient();
        _ = services.ConfigureSettings<StateStoreSettings>(configuration);
        services.TryAddScoped<IStateStoreProvider, DaprClientStateStoreProvider>();
        return services;
    }
}