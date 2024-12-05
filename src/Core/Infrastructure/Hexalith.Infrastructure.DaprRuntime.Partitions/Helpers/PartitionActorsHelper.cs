// <copyright file="PartitionActorsHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Partitions.Helpers;

using System;
using System.Diagnostics.CodeAnalysis;

using Dapr.Actors.Client;
using Dapr.Actors.Runtime;

using Hexalith.Application.Partitions.Configurations;
using Hexalith.Application.Partitions.Services;
using Hexalith.Extensions.Configuration;
using Hexalith.Infrastructure.DaprRuntime.Actors;
using Hexalith.Infrastructure.DaprRuntime.Helpers;
using Hexalith.Infrastructure.DaprRuntime.Partitions.Actors;
using Hexalith.Infrastructure.DaprRuntime.Partitions.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Provides helper methods for partition actors.
/// </summary>
public static class PartitionActorsHelper
{
    /// <summary>
    /// Gets the collection ID for all partitions.
    /// </summary>
    public static string AllPartitionsCollectionId => "All";

    /// <summary>
    /// Adds partition services to the specified IServiceCollection.
    /// </summary>
    /// <param name="services">The IServiceCollection to add services to.</param>
    /// <param name="configuration">The configuration to use for the partition settings.</param>
    /// <returns>The updated IServiceCollection.</returns>
    /// <exception cref="ArgumentNullException">Thrown when services is null.</exception>
    public static IServiceCollection AddPartitions(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);
        _ = services
            .ConfigureSettings<PartitionSettings>(configuration)
            .AddSingleton<IPartitionService, PartitionService>();
        return services;
    }

    /// <summary>
    /// Creates a proxy for the actor that handles all partitions.
    /// </summary>
    /// <param name="actorProxyFactory">The actor proxy factory to use for creating the proxy.</param>
    /// <returns>A proxy for the actor that handles all partitions.</returns>
    /// <exception cref="ArgumentNullException">Thrown when actorProxyFactory is null.</exception>
    public static IKeyHashActor CreateAllPartitionsProxy([NotNull] this IActorProxyFactory actorProxyFactory)
    {
        ArgumentNullException.ThrowIfNull(actorProxyFactory);
        return actorProxyFactory.CreateActorProxy<IKeyHashActor>(
            AllPartitionsCollectionId.ToActorId(),
            IPartitionActor.ActorCollectionName);
    }

    /// <summary>
    /// Registers partition actors with the specified ActorRegistrationCollection.
    /// </summary>
    /// <param name="actorRegistrationCollection">The ActorRegistrationCollection to register actors with.</param>
    /// <exception cref="ArgumentNullException">Thrown when actorRegistrationCollection is null.</exception>
    public static void RegisterPartitionActors(this ActorRegistrationCollection actorRegistrationCollection)
    {
        ArgumentNullException.ThrowIfNull(actorRegistrationCollection);
        actorRegistrationCollection.RegisterActor<PartitionActor>(IPartitionActor.ActorName);
        actorRegistrationCollection.RegisterActor<KeyHashActor>(IPartitionActor.ActorCollectionName);
    }
}