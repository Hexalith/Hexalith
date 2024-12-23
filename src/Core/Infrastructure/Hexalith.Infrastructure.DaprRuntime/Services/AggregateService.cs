// <copyright file="AggregateService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Services;

using System;
using System.Text.Json;
using System.Threading.Tasks;

using Dapr.Actors.Client;

using Hexalith.Application.Metadatas;
using Hexalith.Application.Services;
using Hexalith.Domain.Events;
using Hexalith.Infrastructure.DaprRuntime.Actors;
using Hexalith.Infrastructure.DaprRuntime.Helpers;

/// <summary>
/// Service for handling aggregate operations.
/// </summary>
public class AggregateService : IAggregateService
{
    private readonly IActorProxyFactory _actorProxy;

    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateService"/> class.
    /// </summary>
    /// <param name="actorProxy"></param>
    public AggregateService(IActorProxyFactory actorProxy)
    {
        ArgumentNullException.ThrowIfNull(actorProxy);
        _actorProxy = actorProxy;
    }

    /// <inheritdoc/>
    public async Task<SnapshotEvent?> GetSnapshotAsync(string aggregateName, string partitionId, string id)
    {
        return await GetSnapshotAsync(
            aggregateName,
            Metadata.CreateAggregateGlobalId(partitionId, aggregateName, id));
    }

    /// <inheritdoc/>
    public async Task<SnapshotEvent?> GetSnapshotAsync(string aggregateName, string globalId)
    {
        IDomainAggregateActor actor = _actorProxy.ToDomainAggregateActor(aggregateName, globalId);
        Application.States.MessageState? state = await actor.GetSnapshotEventAsync();
        return state is null ? null : JsonSerializer.Deserialize<SnapshotEvent>(state.Message);
    }
}