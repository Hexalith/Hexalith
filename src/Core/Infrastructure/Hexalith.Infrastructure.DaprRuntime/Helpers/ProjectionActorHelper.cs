// <copyright file="ProjectionActorHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Helpers;

using System;

using Dapr.Actors.Client;
using Dapr.Actors.Runtime;

using Hexalith.Application.Projections;
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
    /// <returns>IServiceCollection.</returns>
    /// <exception cref="ArgumentNullException">null.</exception>
    public static IServiceCollection AddActorProjectionFactory<TState>(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);
        services.TryAddScoped<IProjectionFactory<TState>>(s
            => new ActorProjectionFactory<TState>(s.GetRequiredService<IActorProxyFactory>()));
        return services;
    }

    /// <summary>
    /// Gets the name of the projection actor.
    /// </summary>
    /// <typeparam name="TState">The type of the t state.</typeparam>
    /// <returns>System.String.</returns>
    /// <exception cref="ArgumentNullException">null.</exception>
    public static string GetProjectionActorName<TState>() => typeof(TState).Name + "Projection";

    /// <summary>
    /// Gets the name of the projection actor.
    /// </summary>
    /// <param name="projectionName">Name of the projection.</param>
    /// <param name="applicationName">Name of the application.</param>
    /// <returns>System.String.</returns>
    /// <exception cref="ArgumentNullException">null.</exception>
    public static string GetProjectionActorName(string projectionName, string applicationName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(applicationName);
        ArgumentException.ThrowIfNullOrWhiteSpace(projectionName);
        return applicationName + projectionName;
    }

    /// <summary>
    /// Registers the projection actor.
    /// </summary>
    /// <typeparam name="TState">The type of the t state.</typeparam>
    /// <param name="actorRegistrationCollection">The actor registration collection.</param>
    /// <exception cref="ArgumentNullException">null.</exception>
    public static void RegisterProjectionActor<TState>(this ActorRegistrationCollection actorRegistrationCollection)
    {
        ArgumentNullException.ThrowIfNull(actorRegistrationCollection);
        if (actorRegistrationCollection.Any(p =>
            p.Type.ActorTypeName == GetProjectionActorName<TState>()))
        {
            return;
        }

        actorRegistrationCollection.RegisterActor<KeyValueActor>(GetProjectionActorName<TState>());
    }
}