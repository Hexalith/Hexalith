// <copyright file="PartitionActor.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Partitions.Actors;

using System.Collections.Generic;
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
    public async Task AddAsync(string id, string name)
    {
        IKeyListActor collection = ActorProxy.DefaultProxyFactory.CreateActorProxy<IKeyListActor>(
            new ActorId("Ids"),
            _collectionActorName);
        await collection.AddAsync(id);
        _partition = new Partition(id, name, false);
        await SaveAsync();
    }

    /// <inheritdoc/>
    public async Task DisableAsync(string id)
    {
        Partition partition = _partition ??= await GetPartitionAsync();
        if (!partition.Disabled)
        {
            _partition = partition with { Disabled = true };
            await SaveAsync();
        }
    }

    /// <inheritdoc/>
    public async Task EnableAsync(string id)
    {
        Partition partition = _partition ??= await GetPartitionAsync();
        if (partition.Disabled)
        {
            _partition = partition with { Disabled = false };
            await SaveAsync();
        }
    }

    /// <inheritdoc/>
    public async Task<Partition> GetAsync(string id)
        => _partition ??= await GetPartitionAsync();

    /// <inheritdoc/>
    public Task<IEnumerable<string>> GetIdsAsync() => throw new NotImplementedException();

    /// <inheritdoc/>
    public async Task RenameAsync(string id, string newName)
    {
        Partition partition = _partition ??= await GetPartitionAsync();
        if (partition.Name != newName)
        {
            _partition = partition with { Name = newName };
            await SaveAsync();
        }
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