// ***********************************************************************
// Assembly         : Bspk.InventoryOnHands
// Author           : Jérôme Piquot
// Created          : 08-30-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-30-2023
// ***********************************************************************
// <copyright file="InventoryItemActorService.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.DaprRuntime.Inventories.Services;

using System.Threading;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Client;

using Hexalith.Application.Inventories.Services;
using Hexalith.Domain.Events;
using Hexalith.Infrastructure.DaprRuntime.Inventories.Actors;

/// <summary>
/// Class InventoryItemActorService.
/// Implements the <see cref="IInventoryItemActorService" />.
/// </summary>
/// <seealso cref="IInventoryItemActorService" />
public class InventoryItemActorService : IInventoryItemQueryService
{
    /// <inheritdoc/>
    public async Task<InventoryItemInformationChanged?> CreateInformationChangedEventAsync(string aggregateId, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrEmpty(aggregateId);
        return await GetActor(aggregateId)
            .CreateInformationChangedEventAsync()
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<bool> ExistAsync(string aggregateId, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrEmpty(aggregateId);
        return await GetActor(aggregateId)
            .ExistAsync()
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<string?> GetItemNameAsync(string aggregateId, CancellationToken none)
    {
        ArgumentException.ThrowIfNullOrEmpty(aggregateId);
        return await GetActor(aggregateId)
            .GetItemNameAsync()
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<bool> HasChangesAsync(InventoryItemInformationChanged change, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(change);
        return await GetActor(change.AggregateId)
            .HasChangesAsync(change)
            .ConfigureAwait(false);
    }

    private static IInventoryItemAggregateActor GetActor(string aggregateId)
    {
        return ActorProxy.Create<IInventoryItemAggregateActor>(
            new ActorId(aggregateId),
            nameof(IInventoryItemAggregateActor)[1..]);
    }
}