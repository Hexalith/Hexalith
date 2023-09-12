// ***********************************************************************
// Assembly         : Bspk.Infrastructure.Dynamics365Finance.DaprCloud
// Author           : Jérôme Piquot
// Created          : 01-02-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 03-11-2023
// ***********************************************************************
// <copyright file="InventoryItemStockAggregateActor.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Infrastructure.DaprRuntime.Inventories.Actors;

using System;

using Dapr.Actors.Runtime;

using Hexalith.Application.Aggregates;
using Hexalith.Application.States;
using Hexalith.Application.Tasks;
using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Events;
using Hexalith.Infrastructure.DaprRuntime.Handlers;
using Hexalith.Infrastructure.DaprRuntime.Inventories.Configurations;
using Hexalith.Infrastructure.DaprRuntime.States;

using Microsoft.Extensions.Options;

/// <summary>
/// Logistics partner catalog item aggregate actor interface <see cref="BspkSalesInvoice" />.
/// Extends the <see cref="IActor" />.
/// </summary>
public class InventoryItemStockAggregateActor : Actor, ICommandProcessorActor, IRemindable, IInventoryItemStockAggregateActor
{
    /// <summary>
    /// The settings.
    /// </summary>
    private readonly InventoryItemStockSettings _settings;

    /// <summary>
    /// The state manager.
    /// </summary>
    private readonly IAggregateStateManager _stateManager;

    /// <summary>
    /// The state provider.
    /// </summary>
    private readonly IStateStoreProvider _stateProvider;

    private InventoryItemStock? _aggregate;

    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryItemStockAggregateActor" /> class.
    /// </summary>
    /// <param name="host">The <see cref="ActorHost" /> that will host this actor instance.</param>
    /// <param name="settings">The settings.</param>
    /// <param name="stateManager">The state manager.</param>
    /// <exception cref="ArgumentNullException">Argument null.</exception>
    public InventoryItemStockAggregateActor(
        ActorHost host,
        IOptions<InventoryItemStockSettings> settings,
        IAggregateStateManager stateManager)
        : base(host)
    {
        ArgumentNullException.ThrowIfNull(host);
        ArgumentNullException.ThrowIfNull(stateManager);
        ArgumentNullException.ThrowIfNull(settings?.Value);
        _stateManager = stateManager;
        _stateProvider = new ActorStateStoreProvider(StateManager);
        _settings = settings.Value;
    }

    /// <inheritdoc/>
    public async Task DoAsync(ActorCommandEnvelope envelope)
    {
        ArgumentNullException.ThrowIfNull(envelope);
        await _stateManager
            .AddCommandAsync(
                _stateProvider,
                envelope.Commands.ToArray(),
                envelope.Metadatas.ToArray(),
                RegisterReminderAsync,
                CancellationToken.None)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<bool> ExistAsync()
    {
        if (!await HasCommandsAsync().ConfigureAwait(false))
        {
            return false;
        }

        InventoryItemStock? inventoryItem = await GetAggregateAsync().ConfigureAwait(false);
        return inventoryItem != null;
    }

    /// <inheritdoc/>
    public async Task<bool> HasCommandsAsync()
    {
        return await _stateManager
            .GetCommandCountAsync(_stateProvider, CancellationToken.None)
            .ConfigureAwait(false) > 0L;
    }

    /// <inheritdoc/>
    public async Task<decimal> LevelAsync()
    {
        if (!await HasCommandsAsync().ConfigureAwait(false))
        {
            return 0m;
        }

        InventoryItemStock? stock = await GetAggregateAsync().ConfigureAwait(false);
        return stock == null ? 0m : stock.Quantity;
    }

    /// <inheritdoc/>
    public async Task ReceiveReminderAsync(string reminderName, byte[] state, TimeSpan dueTime, TimeSpan period)
    {
        _aggregate = (InventoryItemStock?)await _stateManager
            .ContinueAsync(
                _stateProvider,
                _settings.ExecuteCommandResiliencyPolicy ?? ResiliencyPolicy.CreateEternalExponentialRetry(),
                _aggregate,
                (e) => new InventoryItemStock((InventoryItemStockIncreased)e),
                RegisterReminderAsync,
                UnregisterReminderAsync,
                CancellationToken.None)
            .ConfigureAwait(false);
    }

    private async Task<InventoryItemStock?> GetAggregateAsync()
    {
        _aggregate ??= (InventoryItemStock?)await _stateManager
                .GetAggregateAsync(
                    _stateProvider,
                    (e) => new InventoryItemStock((InventoryItemStockIncreased)e),
                    CancellationToken.None)
                .ConfigureAwait(false);

        return _aggregate;
    }
}