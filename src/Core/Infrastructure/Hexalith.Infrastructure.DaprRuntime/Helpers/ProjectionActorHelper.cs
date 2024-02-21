// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprRuntime
// Author           : Jérôme Piquot
// Created          : 12-19-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 12-19-2023
// ***********************************************************************
// <copyright file="ProjectionActorHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

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
    /// <param name="applicationName">Name of the application.</param>
    /// <returns>IServiceCollection.</returns>
    /// <exception cref="ArgumentNullException">null.</exception>
    public static IServiceCollection AddActorProjectionFactory<TState>(this IServiceCollection services, string applicationName)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentException.ThrowIfNullOrWhiteSpace(applicationName);
        services.TryAddScoped<IActorProjectionFactory<TState>>(s
            => new ActorProjectionFactory<TState>(s.GetRequiredService<IActorProxyFactory>(), applicationName));
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
    /// <param name="applicationName">Name of the application.</param>
    /// <exception cref="ArgumentNullException">null.</exception>
    public static void RegisterProjectionActor<TState>(this ActorRegistrationCollection actorRegistrationCollection, string applicationName)
    {
        ArgumentNullException.ThrowIfNull(actorRegistrationCollection);
        ArgumentException.ThrowIfNullOrWhiteSpace(applicationName);
        actorRegistrationCollection.RegisterActor<KeyValueActor>(GetProjectionActorName<TState>(applicationName));
    }
}