// <copyright file="PartitionActor.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Partitions.Actors;

using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Client;
using Dapr.Actors.Runtime;

using Hexalith.Application.Partitions.Models;
using Hexalith.Infrastructure.DaprRuntime.Actors;

/// <summary>
/// Represents an actor that manages partitions.
/// </summary>
public class PartitionActor : Actor, IPartitionActor
{
    private const string _collectionActorName = nameof(Partitions);
    private const string _stateName = "State";
    private Partition? _partition;

    /// <summary>
    /// Initializes a new instance of the <see cref="PartitionActor"/> class.
    /// </summary>
    /// <param name="host">The actor host.</param>
    public PartitionActor(ActorHost host)
        : base(host) => ArgumentNullException.ThrowIfNull(host);

    /// <inheritdoc/>
    public async Task DisableAsync()
    {
        Partition partition = _partition ??= await GetPartitionAsync();
        if (!partition.Disabled)
        {
            _partition = partition with { Disabled = true };
            await SaveAsync();
        }
    }

    /// <inheritdoc/>
    public async Task EnableAsync()
    {
        Partition partition = _partition ??= await GetPartitionAsync();
        if (partition.Disabled)
        {
            _partition = partition with { Disabled = false };
            await SaveAsync();
        }
    }

    /// <inheritdoc/>
    public async Task<bool> EnabledAsync()
    {
        if (_partition is null)
        {
            ConditionalValue<Partition> result = await StateManager.TryGetStateAsync<Partition>(_stateName);
            if (!result.HasValue)
            {
                return false;
            }

            _partition = result.Value;
        }

        return !_partition.Disabled;
    }

    /// <inheritdoc/>
    public async Task<Partition> GetAsync()
        => _partition ??= await GetPartitionAsync();

    /// <inheritdoc/>
    public async Task RenameAsync(string newName)
    {
        Partition partition = _partition ??= await GetPartitionAsync();
        if (partition.Name != newName)
        {
            _partition = partition with { Name = newName };
            await SaveAsync();
        }
    }

    /// <inheritdoc/>
    public async Task SetAsync(Partition partition)
    {
        IKeyHashActor collection = ActorProxy.DefaultProxyFactory.CreateActorProxy<IKeyHashActor>(
            new ActorId("Ids"),
            _collectionActorName);
        _ = await collection.AddAsync(partition.Id);
        _partition = partition;
        await SaveAsync();
    }

    private async Task<Partition> GetPartitionAsync()
        => await StateManager.GetStateAsync<Partition>(_stateName);

    private async Task SaveAsync()
    {
        await StateManager.SetStateAsync(
            _stateName,
            _partition ?? throw new InvalidOperationException("The state is not initialized."));
        await StateManager.SaveStateAsync();
    }
}