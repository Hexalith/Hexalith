// ***********************************************************************
// Assembly         : Bspk.Infrastructure.Dynamics365Finance.DaprCloud
// Author           : Jérôme Piquot
// Created          : 01-02-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-09-2023
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
using Hexalith.Application.Configurations;
using Hexalith.Application.States;
using Hexalith.Application.Tasks;
using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Events;
using Hexalith.Infrastructure.DaprRuntime.Handlers;
using Hexalith.Infrastructure.DaprRuntime.Inventories.Configurations;
using Hexalith.Infrastructure.DaprRuntime.States;
using Hexalith.Infrastructure.DaprRuntime.Tasks;

using Microsoft.Extensions.Options;

/// <summary>
/// Logistics partner catalog item aggregate actor interface <see cref="BspkSalesInvoice" />.
/// Extends the <see cref="IActor" />.
/// </summary>
public class InventoryItemAggregateActor : Actor, ICommandProcessorActor, IRemindable, IInventoryItemAggregateActor
{
    /// <summary>
    /// The command processor settings.
    /// </summary>
    private readonly CommandProcessorSettings _commandProcessorSettings;

    /// <summary>
    /// The retry manager.
    /// </summary>
    private readonly IRetryCallbackManager _retryManager;

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

    /// <summary>
    /// The aggregate.
    /// </summary>
    private InventoryItem? _aggregate;

    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryItemAggregateActor" /> class.
    /// </summary>
    /// <param name="host">The <see cref="ActorHost" /> that will host this actor instance.</param>
    /// <param name="settings">The settings.</param>
    /// <param name="stateManager">The state manager.</param>
    /// <exception cref="System.ArgumentNullException">null.</exception>
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
        _commandProcessorSettings = _settings.CommandProcessor ?? new CommandProcessorSettings();
        _retryManager = new DaprRetryCallbackManager(
            this,
            _commandProcessorSettings.ActiveCommandCheckPeriod,
            _commandProcessorSettings.ResiliencyPolicy.Timeout,
            Logger);
    }

    /// <summary>
    /// Continue as an asynchronous operation.
    /// </summary>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public async Task ContinueAsync()
    {
        _aggregate = (InventoryItem?)await _stateManager
            .ContinueAsync(
                _stateProvider,
                _retryManager,
                _commandProcessorSettings.ResiliencyPolicy,
                _aggregate,
                (e) => new InventoryItem((InventoryItemAdded)e),
                Logger,
                CancellationToken.None)
            .ConfigureAwait(false);
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
                _retryManager,
                envelope.Commands.ToArray(),
                envelope.Metadatas.ToArray(),
                Logger,
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
    public async Task<string?> GetItemNameAsync()
        => (await GetAggregateAsync().ConfigureAwait(false))?.Name;

    /// <inheritdoc/>
    public async Task<bool> HasChangesAsync(InventoryItemInformationChanged changed)
    {
        ArgumentNullException.ThrowIfNull(changed);

        InventoryItem? inventoryItem = await GetAggregateAsync().ConfigureAwait(false);
        return inventoryItem == null
            ? throw new InvalidOperationException($"InventoryItem {Id.GetId()} not found.")
            :
            inventoryItem.Name == changed.Name;
    }

    /// <inheritdoc/>
    public async Task<bool> HasCommandsAsync()
    {
        return await _stateManager
            .GetCommandCountAsync(_stateProvider, CancellationToken.None)
            .ConfigureAwait(false) > 0L;
    }

    /// <inheritdoc/>
    public async Task ReceiveReminderAsync(string reminderName, byte[] state, TimeSpan dueTime, TimeSpan period) => await ContinueAsync();

    /// <summary>
    /// Get aggregate as an asynchronous operation.
    /// </summary>
    /// <returns>A Task&lt;InventoryItem&gt; representing the asynchronous operation.</returns>
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