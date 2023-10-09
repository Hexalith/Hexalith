// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprRuntime.Parties
// Author           : Jérôme Piquot
// Created          : 01-02-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-04-2023
// ***********************************************************************
// <copyright file="CustomerAggregateActor.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Infrastructure.DaprRuntime.Parties.Actors;

using System;

using Dapr.Actors.Runtime;

using Hexalith.Application.Aggregates;
using Hexalith.Application.Configurations;
using Hexalith.Application.Exceptions;
using Hexalith.Application.Parties.Commands;
using Hexalith.Application.States;
using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Events;
using Hexalith.Domain.ValueObjets;
using Hexalith.Infrastructure.DaprRuntime.Handlers;
using Hexalith.Infrastructure.DaprRuntime.Helpers;
using Hexalith.Infrastructure.DaprRuntime.Parties.Configurations;
using Hexalith.Infrastructure.DaprRuntime.States;

using Microsoft.Extensions.Options;

/// <summary>
/// Logistics partner catalog item aggregate actor interface <see cref="BspkSalesInvoice" />.
/// Extends the <see cref="IActor" />.
/// </summary>
public class CustomerAggregateActor : Actor, ICommandProcessorActor, IRemindable, ICustomerAggregateActor
{
    /// <summary>
    /// The command processor settings.
    /// </summary>
    private readonly CommandProcessorSettings _commandProcessorSettings;

    /// <summary>
    /// The settings.
    /// </summary>
    private readonly CustomerSettings _settings;

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
    private Customer? _aggregate;

    /// <summary>
    /// The timer.
    /// </summary>
    private ActorTimer? _timer;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerAggregateActor" /> class.
    /// </summary>
    /// <param name="host">The <see cref="ActorHost" /> that will host this actor instance.</param>
    /// <param name="settings">The settings.</param>
    /// <param name="stateManager">The state manager.</param>
    /// <exception cref="ArgumentNullException">Argument null.</exception>
    public CustomerAggregateActor(
        ActorHost host,
        IOptions<CustomerSettings> settings,
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
    }

    /// <summary>
    /// Continue as an asynchronous operation.
    /// </summary>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public async Task ContinueAsync()
    {
        _aggregate = (Customer?)await _stateManager
            .ContinueAsync(
                _stateProvider,
                _commandProcessorSettings.ResiliencyPolicy,
                _aggregate,
                (e) => new Customer((CustomerRegistered)e),
                RegisterContinueCallbackAsync,
                UnregisterContinueCallbackAsync,
                CancellationToken.None)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<CustomerInformationChanged?> CreateInformationChangedEventAsync()
    {
        Customer? customer = await GetAggregateAsync().ConfigureAwait(false);
        return customer == null
            ? null
            : new CustomerInformationChanged(
                customer.CompanyId,
                customer.Id,
                customer.Name,
                customer.Contact,
                customer.WarehouseId,
                customer.CommissionSalesGroupId,
                DateTimeOffset.UtcNow);
    }

    /// <inheritdoc/>
    public async Task DoAsync(ActorCommandEnvelope envelope)
    {
        ArgumentNullException.ThrowIfNull(envelope);
        _aggregate = null;
        await _stateManager
            .AddCommandAsync(
                _stateProvider,
                envelope.Commands.ToArray(),
                envelope.Metadatas.ToArray(),
                RegisterContinueCallbackAsync,
                CancellationToken.None)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<bool> ExistAsync()
        => await GetAggregateAsync() != null;

    /// <inheritdoc/>
    public async Task<bool> HasChangesAsync(ChangeCustomerInformation change)
    {
        ArgumentNullException.ThrowIfNull(change);
        if (change.AggregateId != Id.GetId())
        {
            throw new InvalidCommandAggregateIdException(Id.GetId(), change);
        }

        Customer? customer = await GetAggregateAsync().ConfigureAwait(false);
        return customer == null
            ? throw new InvalidOperationException($"Customer {Id.GetId()} not found.")
            :
            customer.Name == change.Name &&
            customer.CommissionSalesGroupId == change.CommissionSalesGroupId &&
            customer.WarehouseId == change.WarehouseId &&
            Contact.AreSame(customer.Contact, change.Contact);
    }

    /// <inheritdoc/>
    public async Task<bool> HasCommandsAsync()
    {
        return await _stateManager
            .GetCommandCountAsync(_stateProvider, CancellationToken.None)
            .ConfigureAwait(false) > 0L;
    }

    /// <inheritdoc/>
    public async Task<bool> IsIntercompanyDirectDeliveryAsync()
    {
        Customer? customer = await GetAggregateAsync().ConfigureAwait(false);
        return customer == null
            ? throw new InvalidOperationException($"Customer with AggregateId={Id.GetId()} not found.")
            : customer.IntercompanyDirectDelivery;
    }

    /// <inheritdoc/>
    public async Task ReceiveReminderAsync(string reminderName, byte[] state, TimeSpan dueTime, TimeSpan period) => await ContinueAsync();

    /// <summary>
    /// Get aggregate as an asynchronous operation.
    /// </summary>
    /// <returns>A Task&lt;Customer&gt; representing the asynchronous operation.</returns>
    private async Task<Customer?> GetAggregateAsync()
    {
        return _aggregate ??= (Customer?)await _stateManager
                .GetAggregateAsync(
                    _stateProvider,
                    (e) => new Customer((CustomerRegistered)e),
                    CancellationToken.None)
                .ConfigureAwait(false);
    }

    /// <summary>
    /// Register continue callback as an asynchronous operation.
    /// </summary>
    /// <param name="dueTime">The due time.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    private async Task RegisterContinueCallbackAsync(TimeSpan dueTime)
    {
        (_, _timer) = await this.RegisterContinueCallbackReminderAsync(
        dueTime,
        _commandProcessorSettings.ActiveCommandCheckPeriod,
        _commandProcessorSettings.ResiliencyPolicy.Timeout,
        _timer != null,
        Logger);
    }

    /// <summary>
    /// Unregister continue callback as an asynchronous operation.
    /// </summary>
    /// <returns>A Task representing the asynchronous operation.</returns>
    private async Task UnregisterContinueCallbackAsync()
        => await this.UnregisterContinueCallbackReminderAsync(
            _timer != null,
            GetReminderAsync,
            UnregisterReminderAsync,
            UnregisterTimerAsync);
}