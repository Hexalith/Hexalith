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
namespace Hexalith.Infrastructure.DaprRuntime.ExternalSystems.Actors;

using System;

using Dapr.Actors.Runtime;

using Hexalith.Application.Aggregates;
using Hexalith.Application.Configurations;
using Hexalith.Application.States;
using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Events;
using Hexalith.Infrastructure.DaprRuntime.ExternalSystems.Configurations;
using Hexalith.Infrastructure.DaprRuntime.Handlers;
using Hexalith.Infrastructure.DaprRuntime.Helpers;
using Hexalith.Infrastructure.DaprRuntime.States;

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

    /// <summary>
    /// The settings.
    /// </summary>
    private readonly AggregateExternalReferenceSettings _settings;

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
    /// The reminder.
    /// </summary>
    private IActorReminder? _reminder;

    /// <summary>
    /// The timer.
    /// </summary>
    private ActorTimer? _timer;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalSystemReferenceAggregateActor" /> class.
    /// </summary>
    /// <param name="host">The <see cref="ActorHost" /> that will host this actor instance.</param>
    /// <param name="settings">The settings.</param>
    /// <param name="stateManager">The state manager.</param>
    /// <exception cref="ArgumentNullException">Argument null.</exception>
    public ExternalSystemReferenceAggregateActor(
        ActorHost host,
        IOptions<AggregateExternalReferenceSettings> settings,
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

    /// <inheritdoc/>
    public async Task DoAsync(ActorCommandEnvelope envelope)
    {
        ArgumentNullException.ThrowIfNull(envelope);
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
    public async Task<string?> GetIdAsync()
    {
        if (_aggregate == null)
        {
            _aggregate = (ExternalSystemReference?)await _stateManager
                .GetAggregateAsync(
                    _stateProvider,
                    (e) => new ExternalSystemReference((ExternalSystemReferenceAdded)e),
                    CancellationToken.None)
                .ConfigureAwait(false);
        }

        return _aggregate?.ReferenceAggregateId;
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
        _aggregate = (ExternalSystemReference?)await _stateManager
            .ContinueAsync(
                _stateProvider,
                _commandProcessorSettings.ResiliencyPolicy,
                _aggregate,
                (e) => new ExternalSystemReference((ExternalSystemReferenceAdded)e),
                RegisterContinueCallbackAsync,
                UnregisterContinueCallbackAsync,
                CancellationToken.None)
            .ConfigureAwait(false);
    }

    private async Task RegisterContinueCallbackAsync(TimeSpan dueTime)
    {
        (_reminder, _timer) = await this.RegisterContinueCallbackReminderAsync(
        dueTime,
        _commandProcessorSettings.ActiveCommandCheckPeriod,
        _commandProcessorSettings.ResiliencyPolicy.Timeout,
        _timer != null,
        GetReminderAsync,
        RegisterReminderAsync,
        UnregisterTimerAsync);
    }

    private async Task UnregisterContinueCallbackAsync()
        => await this.UnregisterContinueCallbackReminderAsync(
            _timer != null,
            GetReminderAsync,
            UnregisterReminderAsync,
            UnregisterTimerAsync);
}