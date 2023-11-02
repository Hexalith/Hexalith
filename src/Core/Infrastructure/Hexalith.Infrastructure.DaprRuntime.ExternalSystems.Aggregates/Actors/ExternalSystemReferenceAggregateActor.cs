// ***********************************************************************
// Assembly         : Bspk.Infrastructure.Dynamics365Finance.DaprCloud
// Author           : Jérôme Piquot
// Created          : 01-02-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 09-12-2023
// ***********************************************************************
// <copyright file="ExternalSystemReferenceAggregateActor.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Infrastructure.DaprRuntime.ExternalSystems.Aggregates.Actors;

using System;

using Dapr.Actors.Runtime;

using Hexalith.Application.Aggregates;
using Hexalith.Application.Configurations;
using Hexalith.Application.States;
using Hexalith.Application.Tasks;
using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Events;
using Hexalith.Infrastructure.DaprRuntime.ExternalSystems.Actors;
using Hexalith.Infrastructure.DaprRuntime.ExternalSystems.Configurations;
using Hexalith.Infrastructure.DaprRuntime.Handlers;
using Hexalith.Infrastructure.DaprRuntime.States;
using Hexalith.Infrastructure.DaprRuntime.Tasks;

using Microsoft.Extensions.Options;

/// <summary>
/// Logistics partner catalog item aggregate actor interface <see cref="BspkSalesInvoice" />.
/// Extends the <see cref="IActor" />.
/// </summary>
public class ExternalSystemReferenceAggregateActor : Actor, ICommandProcessorActor, IRemindable, IExternalSystemReferenceAggregateActor
{
    /// <summary>
    /// The command processor settings.
    /// </summary>
    private readonly CommandProcessorSettings _commandProcessorSettings;

    private readonly IRetryCallbackManager _retryManager;

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
    private ExternalSystemReference? _aggregate;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalSystemReferenceAggregateActor" /> class.
    /// </summary>
    /// <param name="host">The <see cref="ActorHost" /> that will host this actor instance.</param>
    /// <param name="settings">The settings.</param>
    /// <param name="stateManager">The state manager.</param>
    /// <exception cref="ArgumentNullException">Argument null.</exception>
    public ExternalSystemReferenceAggregateActor(
        ActorHost host,
        IOptions<ExternalSystemReferenceSettings> settings,
        IAggregateStateManager stateManager)
        : base(host)
    {
        ArgumentNullException.ThrowIfNull(host);
        ArgumentNullException.ThrowIfNull(stateManager);
        ArgumentNullException.ThrowIfNull(settings?.Value);
        _stateManager = stateManager;
        _stateProvider = new ActorStateStoreProvider(StateManager);
        _commandProcessorSettings = settings.Value.CommandProcessor ?? new CommandProcessorSettings();
        _retryManager = new DaprRetryCallbackManager(
            this,
            _commandProcessorSettings.ActiveCommandCheckPeriod,
            _commandProcessorSettings.ResiliencyPolicy.Timeout,
            Logger);
    }

    /// <inheritdoc/>
    public async Task ContinueAsync()
    {
        _aggregate = (ExternalSystemReference?)await _stateManager
            .ContinueAsync(
                _stateProvider,
                _retryManager,
                _commandProcessorSettings.ResiliencyPolicy,
                _aggregate,
                (e) => new ExternalSystemReference((ExternalSystemReferenceAdded)e),
                CancellationToken.None)
            .ConfigureAwait(false);
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
        _aggregate = await GetAggregateAsync().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<string?> GetReferenceAggregateIdAsync()
    {
        return _aggregate == null && !await HasCommandsAsync().ConfigureAwait(false)
            ? null
            : (await GetAggregateAsync().ConfigureAwait(false)).ReferenceAggregateId;
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
        => await ContinueAsync().ConfigureAwait(false);

    /// <summary>
    /// Get aggregate as an asynchronous operation.
    /// </summary>
    /// <returns>A Task&lt;ExternalSystemReference&gt; representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">Aggregate {Host.ActorTypeInfo.ActorTypeName} {Id.GetId()} is not ready. Check task processor failure information.</exception>
    private async Task<ExternalSystemReference> GetAggregateAsync()
    {
        return _aggregate ??= (ExternalSystemReference?)await _stateManager
            .ContinueAsync(
                 _stateProvider,
                 _retryManager,
                 _commandProcessorSettings.ResiliencyPolicy,
                 _aggregate,
                 (e) => new ExternalSystemReference((ExternalSystemReferenceAdded)e),
                 CancellationToken.None)
            .ConfigureAwait(false)
            ?? throw new InvalidOperationException($"Aggregate {Host.ActorTypeInfo.ActorTypeName} {Id.GetId()} is not ready. Check task processor failure information.");
    }
}