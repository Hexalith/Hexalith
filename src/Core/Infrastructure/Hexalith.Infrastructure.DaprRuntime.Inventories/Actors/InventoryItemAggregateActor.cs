// ***********************************************************************
// Assembly         : Bspk.Infrastructure.Dynamics365Finance.DaprCloud
// Author           : Jérôme Piquot
// Created          : 01-02-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 03-11-2023
// ***********************************************************************
// <copyright file="InventoryItemAggregateActor.cs" company="Fiveforty SAS Paris France">
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
using Hexalith.Application.Exceptions;
using Hexalith.Application.Inventories.Commands;
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
public class InventoryItemAggregateActor : Actor, ICommandProcessorActor, IRemindable, IInventoryItemAggregateActor
{
    /// <summary>
    /// The settings.
    /// </summary>
    private readonly InventoryItemSettings _settings;

    /// <summary>
    /// The state manager.
    /// </summary>
    private readonly IAggregateStateManager _stateManager;

    /// <summary>
    /// The state provider.
    /// </summary>
    private readonly IStateStoreProvider _stateProvider;

    private InventoryItem? _aggregate;

    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryItemAggregateActor" /> class.
    /// </summary>
    /// <param name="host">The <see cref="ActorHost" /> that will host this actor instance.</param>
    /// <param name="settings">The settings.</param>
    /// <param name="stateManager">The state manager.</param>
    /// <exception cref="ArgumentNullException">Argument null.</exception>
    public InventoryItemAggregateActor(
        ActorHost host,
        IOptions<InventoryItemSettings> settings,
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
    public async Task<InventoryItemInformationChanged?> CreateInformationChangedEventAsync()
    {
        InventoryItem? inventoryItem = await GetAggregateAsync().ConfigureAwait(false);
        return inventoryItem == null
            ? null
            : new InventoryItemInformationChanged(
                inventoryItem.CompanyId,
                inventoryItem.Id,
                inventoryItem.Name,
                DateTimeOffset.UtcNow);
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

        InventoryItem? inventoryItem = await GetAggregateAsync().ConfigureAwait(false);
        return inventoryItem != null;
    }

    /// <inheritdoc/>
    public async Task<bool> HasChangesAsync(ChangeInventoryItemInformation change)
    {
        ArgumentNullException.ThrowIfNull(change);
        if (change.AggregateId != Id.ToString())
        {
            throw new InvalidCommandAggregateIdException(Id.ToString(), change);
        }

        InventoryItem? inventoryItem = await GetAggregateAsync().ConfigureAwait(false);
        return inventoryItem == null
            ? throw new InvalidOperationException($"InventoryItem {Id} not found.")
            :
            inventoryItem.Name == change.Name;
    }

    /// <inheritdoc/>
    public async Task<bool> HasCommandsAsync()
    {
        return await _stateManager
            .GetCommandCountAsync(_stateProvider, CancellationToken.None)
            .ConfigureAwait(false) > 0L;
    }

    /// <inheritdoc/>
    public async Task ReceiveReminderAsync(string reminderName, byte[] state, TimeSpan dueTime, TimeSpan period)
    {
        _aggregate = (InventoryItem?)await _stateManager
            .ContinueAsync(
                _stateProvider,
                _settings.ExecuteCommandResiliencyPolicy ?? ResiliencyPolicy.CreateEternalExponentialRetry(),
                _aggregate,
                (e) => new InventoryItem((InventoryItemAdded)e),
                RegisterReminderAsync,
                UnregisterReminderAsync,
                CancellationToken.None)
            .ConfigureAwait(false);
    }

    private async Task<InventoryItem?> GetAggregateAsync()
    {
        _aggregate ??= (InventoryItem?)await _stateManager
                .GetAggregateAsync(
                    _stateProvider,
                    (e) => new InventoryItem((InventoryItemAdded)e),
                    CancellationToken.None)
                .ConfigureAwait(false);

        return _aggregate;
    }
}