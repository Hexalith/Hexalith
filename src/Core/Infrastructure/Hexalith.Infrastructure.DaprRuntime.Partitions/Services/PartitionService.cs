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

using Hexalith.Application.Partitions.Configurations;
using Hexalith.Application.Partitions.Models;
using Hexalith.Application.Partitions.Services;
using Hexalith.Extensions.Configuration;
using Hexalith.Infrastructure.DaprRuntime.Helpers;
using Hexalith.Infrastructure.DaprRuntime.Partitions.Actors;
using Hexalith.Infrastructure.DaprRuntime.Partitions.Helpers;

using Microsoft.Extensions.Options;

/// <summary>
/// Service implementation for retrieving partition information using Dapr actors.
/// </summary>
internal class PartitionService : IPartitionService
{
    private readonly IActorProxyFactory _actorProxyFactory;
    private readonly string _default;

    /// <summary>
    /// Initializes a new instance of the <see cref="PartitionService"/> class.
    /// </summary>
    /// <param name="actorProxyFactory">The actor proxy factory.</param>
    /// <param name="settings">The partition settings.</param>
    public PartitionService(IActorProxyFactory actorProxyFactory, IOptions<PartitionSettings> settings)
    {
        ArgumentNullException.ThrowIfNull(actorProxyFactory);
        ArgumentNullException.ThrowIfNull(settings);
        SettingsException<PartitionSettings>.ThrowIfNullOrWhiteSpace(settings.Value.Default);
        _default = settings.Value.Default;
        _actorProxyFactory = actorProxyFactory;
    }

    /// <inheritdoc/>
    public async Task AddAsync(Partition partition, CancellationToken cancellationToken)
        => await GetPartitionActor(partition.Id).SetAsync(partition);

    /// <inheritdoc/>
    public async Task<string> DefaultAsync(CancellationToken cancellationToken)
    {
        IPartitionActor partitionActor = GetPartitionActor(_default);
        bool enabled = await partitionActor.EnabledAsync();
        if (!enabled)
        {
            await partitionActor.SetAsync(new Partition(_default, _default, false));
        }

        return _default;
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<Partition> GetAllAsync([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (Partition id in GetPartitionsAsync(await _actorProxyFactory.CreateAllPartitionsProxy().AllAsync(0, 0), cancellationToken))
        {
            yield return id;
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<string>> GetAllIdsAsync(CancellationToken cancellationToken)
        => await _actorProxyFactory.CreateAllPartitionsProxy()
            .AllAsync(0, 0);

    private IPartitionActor GetPartitionActor(string partitionId)
        => _actorProxyFactory.CreateActorProxy<IPartitionActor>(partitionId.ToActorId(), IPartitionActor.ActorName);

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