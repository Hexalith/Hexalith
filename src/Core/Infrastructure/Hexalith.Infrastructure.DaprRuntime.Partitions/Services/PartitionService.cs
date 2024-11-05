// <copyright file="PartitionService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Partitions.Services;

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using Dapr.Actors.Client;

using Hexalith.Application.Partitions.Models;
using Hexalith.Application.Partitions.Services;
using Hexalith.Infrastructure.DaprRuntime.Actors;
using Hexalith.Infrastructure.DaprRuntime.Helpers;
using Hexalith.Infrastructure.DaprRuntime.Partitions.Actors;

/// <summary>
/// Service implementation for retrieving partition information using Dapr actors.
/// </summary>
internal class PartitionService : IPartitionService
{
    private IKeyHashActor PartitionCollectionActor
            => ActorProxy.Create<IKeyHashActor>(
                new Dapr.Actors.ActorId(IPartitionActor.ActorName),
                IPartitionActor.ActorCollectionName);

    /// <inheritdoc/>
    public async IAsyncEnumerable<Partition> GetAllAsync([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (Partition id in GetPartitionsAsync(await PartitionCollectionActor.AllAsync(), cancellationToken))
        {
            yield return id;
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<string>> GetAllIdsAsync(CancellationToken cancellationToken)
        => await PartitionCollectionActor
            .AllAsync();

    private IPartitionActor GetPartitionActor(string sessionId)
                    => ActorProxy.Create<IPartitionActor>(
                sessionId.ToActorId(),
                IPartitionActor.ActorCollectionName);

    private async IAsyncEnumerable<Partition> GetPartitionsAsync(
        IEnumerable<string> ids,
        [EnumeratorCancellation]
        CancellationToken cancellationToken)
    {
        foreach (string id in ids)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                yield break;
            }

            IPartitionActor partitionActor = GetPartitionActor(id);
            Partition partition = await partitionActor.GetAsync();
            yield return partition;
        }
    }
}