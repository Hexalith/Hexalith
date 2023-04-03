// ***********************************************************************
// Assembly         : DevOpsAssistant
// Author           : Jérôme Piquot
// Created          : 04-02-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 04-02-2023
// ***********************************************************************
// <copyright file="DevOpsUnitOfWorkAggregateActor.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace DevOpsAssistant.Infrastructure.Actors;

using System;

using Dapr.Actors.Runtime;

using DevOpsAssistant.Application.Configuration;

using Hexalith.Application.Abstractions.Aggregates;
using Hexalith.Application.Abstractions.States;
using Hexalith.Application.Abstractions.Tasks;
using Hexalith.Infrastructure.DaprRuntime.Handlers;
using Hexalith.Infrastructure.DaprRuntime.States;

using Microsoft.Extensions.Options;

/// <summary>
/// Logistics partner catalog item aggregate actor interface <see cref="LogisticsPartnerSalesInvoice" />.
/// Extends the <see cref="IActor" />.
/// </summary>
public class DevOpsUnitOfWorkAggregateActor : Actor, IDevOpsUnitOfWorkAggregateActor, IRemindable
{
    /// <summary>
    /// The settings.
    /// </summary>
    private readonly DevOpsUnitOfWorkSettings _settings;

    /// <summary>
    /// The state manager.
    /// </summary>
    private readonly IAggregateStateManager _stateManager;

    /// <summary>
    /// The state provider.
    /// </summary>
    private readonly IStateStoreProvider _stateProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="DevOpsUnitOfWorkAggregateActor" /> class.
    /// </summary>
    /// <param name="host">The <see cref="T:Dapr.Actors.Runtime.ActorHost" /> that will host this actor instance.</param>
    /// <param name="settings">The settings.</param>
    /// <param name="stateManager">The state manager.</param>
    /// <exception cref="System.ArgumentNullException">Null argument.</exception>
    public DevOpsUnitOfWorkAggregateActor(
        ActorHost host,
        IOptions<DevOpsUnitOfWorkSettings> settings,
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

    /// <summary>
    /// Does the logistics partner catalog item remove.
    /// </summary>
    /// <param name="envelope">The command.</param>
    /// <returns>Task.</returns>
    /// <exception cref="System.ArgumentNullException">Null argument.</exception>
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
    public async Task<bool> HasCommandsAsync()
    {
        return await _stateManager
            .GetCommandCountAsync(_stateProvider, CancellationToken.None)
            .ConfigureAwait(false) > 0;
    }

    /// <inheritdoc/>
    public async Task ReceiveReminderAsync(string reminderName, byte[] state, TimeSpan dueTime, TimeSpan period)
    {
        await _stateManager
            .ContinueAsync(
                _stateProvider,
                _settings.ExecuteCommandResiliencyPolicy ?? ResiliencyPolicy.CreateEternalExponentialRetry(),
                RegisterReminderAsync,
                CancellationToken.None)
            .ConfigureAwait(false);
    }
}