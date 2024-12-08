// <copyright file="IdCollectionFactory.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Services;

using System;

using Dapr.Actors.Client;

using Hexalith.Application.Services;
using Hexalith.Infrastructure.DaprRuntime.Actors;
using Hexalith.Infrastructure.DaprRuntime.Helpers;

/// <summary>
/// Factory class for creating ID collection services.
/// </summary>
public class IdCollectionFactory : IIdCollectionFactory
{
    private readonly IActorProxyFactory _actorProxyFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="IdCollectionFactory"/> class.
    /// </summary>
    /// <param name="actorProxyFactory">The actor proxy factory.</param>
    /// <exception cref="ArgumentNullException">Thrown when actorProxyFactory is null.</exception>
    public IdCollectionFactory(IActorProxyFactory actorProxyFactory)
    {
        ArgumentNullException.ThrowIfNull(actorProxyFactory);
        _actorProxyFactory = actorProxyFactory;
    }

    /// <summary>
    /// Creates a new ID collection service.
    /// </summary>
    /// <param name="collectionName">The name of the collection.</param>
    /// <param name="partitionId">The partition ID.</param>
    /// <returns>An instance of <see cref="IIdCollectionService"/>.</returns>
    public IIdCollectionService CreateService(string collectionName, string partitionId)
    {
        ArgumentNullException.ThrowIfNull(collectionName);
        ArgumentNullException.ThrowIfNull(partitionId);

        ISequentialStringListActor actor = _actorProxyFactory.CreateActorProxy<ISequentialStringListActor>(partitionId.ToActorId(), collectionName);

        // Assuming IdCollectionService is a class that implements IIdCollectionService
        return new IdCollectionService(actor);
    }
}