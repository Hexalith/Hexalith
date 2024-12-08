// <copyright file="IdCollectionService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Services;

using System;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Services;
using Hexalith.Infrastructure.DaprRuntime.Actors;

/// <summary>
/// Service for managing ID collections using a sequential string list actor.
/// </summary>
public class IdCollectionService : IIdCollectionService
{
    private readonly ISequentialStringListActor _listActor;

    /// <summary>
    /// Initializes a new instance of the <see cref="IdCollectionService"/> class.
    /// </summary>
    /// <param name="listActor">The sequential string list actor.</param>
    /// <exception cref="ArgumentNullException">Thrown when listActor is null.</exception>
    public IdCollectionService(ISequentialStringListActor listActor)
    {
        ArgumentNullException.ThrowIfNull(listActor);
        _listActor = listActor;
    }

    /// <summary>
    /// Adds an aggregate global ID to the collection.
    /// </summary>
    /// <param name="aggregateGlobalId">The aggregate global ID to add.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous add operation.</returns>
    public async Task AddAsync(string aggregateGlobalId, CancellationToken cancellationToken)
        => await _listActor.AddAsync(aggregateGlobalId);
}