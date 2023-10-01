// ***********************************************************************
// Assembly         : Bspk.InventoryOnHands
// Author           : Jérôme Piquot
// Created          : 08-30-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 09-12-2023
// ***********************************************************************
// <copyright file="InventoryItemStockActorService.cs" company="Fiveforty SAS Paris France">
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
using Hexalith.Infrastructure.DaprRuntime.Inventories.Actors;

/// <summary>
/// Class InventoryItemStockActorService.
/// Implements the <see cref="IInventoryItemStockActorService" />.
/// </summary>
/// <seealso cref="IInventoryItemStockActorService" />
public class InventoryItemStockActorService : IInventoryItemStockQueryService
{
    /// <inheritdoc/>
    public async Task<bool> ExistAsync(string aggregateId, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrEmpty(aggregateId);
        return await GetActor(aggregateId)
            .ExistAsync()
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<decimal> LevelAsync(string aggregateId, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrEmpty(aggregateId);
        return await GetActor(aggregateId)
            .LevelAsync()
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the actor.
    /// </summary>
    /// <param name="aggregateId">The aggregate identifier.</param>
    /// <returns>IInventoryItemStockAggregateActor.</returns>
    private static IInventoryItemStockAggregateActor GetActor(string aggregateId)
    {
        return ActorProxy.Create<IInventoryItemStockAggregateActor>(
            new ActorId(aggregateId),
            nameof(InventoryItemStockAggregateActor));
    }
}