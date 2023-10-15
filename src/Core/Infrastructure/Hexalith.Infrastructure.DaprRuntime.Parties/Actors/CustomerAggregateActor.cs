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
using Hexalith.Application.Tasks;
using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Events;
using Hexalith.Domain.ValueObjets;
using Hexalith.Infrastructure.DaprRuntime.Handlers;
using Hexalith.Infrastructure.DaprRuntime.Parties.Configurations;
using Hexalith.Infrastructure.DaprRuntime.States;
using Hexalith.Infrastructure.DaprRuntime.Tasks;

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
    /// The retry manager.
    /// </summary>
    private readonly IRetryCallbackManager _retryManager;

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
        _aggregate = (Customer?)await _stateManager
            .ContinueAsync(
                _stateProvider,
                _retryManager,
                _commandProcessorSettings.ResiliencyPolicy,
                _aggregate,
                (e) => new Customer((CustomerRegistered)e),
                CancellationToken.None)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<CustomerInformationChanged?> CreateInformationChangedEventAsync()
    {
        Customer customer = await GetAggregateAsync().ConfigureAwait(false);
        return new CustomerInformationChanged(
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
                _retryManager,
                envelope.Commands.ToArray(),
                envelope.Metadatas.ToArray(),
                CancellationToken.None)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<bool> ExistAsync()
    {
        if (await HasCommandsAsync().ConfigureAwait(false))
        {
            _ = await GetAggregateAsync().ConfigureAwait(false);
            return true;
        }

        return false;
    }

    /// <inheritdoc/>
    public async Task<bool> HasChangesAsync(ChangeCustomerInformation change)
    {
        ArgumentNullException.ThrowIfNull(change);
        if (change.AggregateId != Id.GetId())
        {
            throw new InvalidCommandAggregateIdException(Id.GetId(), change);
        }

        Customer customer = await GetAggregateAsync()
            .ConfigureAwait(false);
        return !(customer.Name == change.Name &&
            customer.CommissionSalesGroupId == change.CommissionSalesGroupId &&
            customer.WarehouseId == change.WarehouseId &&
            Contact.AreSame(customer.Contact, change.Contact));
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
        => (await GetAggregateAsync().ConfigureAwait(false)).IntercompanyDirectDelivery;

    /// <inheritdoc/>
    public async Task ReceiveReminderAsync(string reminderName, byte[] state, TimeSpan dueTime, TimeSpan period)
        => await ContinueAsync().ConfigureAwait(false);

    /// <summary>
    /// Get aggregate as an asynchronous operation.
    /// </summary>
    /// <returns>A Task&lt;Customer&gt; representing the asynchronous operation.</returns>
    /// <exception cref="System.InvalidOperationException">Aggregate {Host.ActorTypeInfo.ActorTypeName} {Id.GetId()} is not ready. Check task processor failure information.</exception>
    private async Task<Customer> GetAggregateAsync()
    {
        _aggregate ??= (Customer?)await _stateManager
                .GetAggregateAsync(
                    _stateProvider,
                    (e) => new Customer((CustomerRegistered)e),
                    CancellationToken.None)
                .ConfigureAwait(false);
        return _aggregate
            ?? throw new InvalidOperationException($"Aggregate {Host.ActorTypeInfo.ActorTypeName} {Id.GetId()} is not ready. Check task processor failure information.");
    }
}