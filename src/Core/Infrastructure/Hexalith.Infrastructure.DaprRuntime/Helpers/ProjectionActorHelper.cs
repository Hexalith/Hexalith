// <copyright file="ProjectionActorHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Helpers;

using System;

using Dapr.Actors.Client;
using Dapr.Actors.Runtime;

using Hexalith.Infrastructure.DaprRuntime.Actors;
using Hexalith.Infrastructure.DaprRuntime.Projections;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// Class ProjectionActorHelper.
/// </summary>
public static class ProjectionActorHelper
{
    /// <summary>
    /// Adds the actor projection factory.
    /// </summary>
    /// <typeparam name="TState">The type of the t state.</typeparam>
    /// <param name="services">The services.</param>
    /// <param name="applicationId">Name of the application.</param>
    /// <returns>IServiceCollection.</returns>
    /// <exception cref="ArgumentNullException">null.</exception>
    public static IServiceCollection AddActorProjectionFactory<TState>(this IServiceCollection services, string applicationId)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentException.ThrowIfNullOrWhiteSpace(applicationId);
        services.TryAddScoped<IActorProjectionFactory<TState>>(s
            => new ActorProjectionFactory<TState>(s.GetRequiredService<IActorProxyFactory>(), applicationId));
        return services;
    }

    /// <summary>
    /// Gets the name of the projection actor.
    /// </summary>
    /// <typeparam name="TState">The type of the t state.</typeparam>
    /// <param name="applicationName">Name of the application.</param>
    /// <returns>System.String.</returns>
    /// <exception cref="ArgumentNullException">null.</exception>
    public static string GetProjectionActorName<TState>(string applicationName)
    {
        ArgumentNullException.ThrowIfNull(applicationName);
        return applicationName + typeof(TState).Name + "Projection";
    }

    /// <summary>
    /// Registers the projection actor.
    /// </summary>
    /// <typeparam name="TState">The type of the t state.</typeparam>
    /// <param name="actorRegistrationCollection">The actor registration collection.</param>
    /// <param name="applicationId">Name of the application.</param>
    /// <exception cref="ArgumentNullException">null.</exception>
    public static void RegisterProjectionActor<TState>(this ActorRegistrationCollection actorRegistrationCollection, string applicationId)
    {
        ArgumentNullException.ThrowIfNull(actorRegistrationCollection);
        ArgumentException.ThrowIfNullOrWhiteSpace(applicationId);
        actorRegistrationCollection.RegisterActor<KeyValueActor>(GetProjectionActorName<TState>(applicationId));
    }
}