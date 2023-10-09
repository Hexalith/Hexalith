// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprRuntime
// Author           : Jérôme Piquot
// Created          : 10-09-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-09-2023
// ***********************************************************************
// <copyright file="DaprRetryCallbackManager.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.DaprRuntime.Tasks;

using System;
using System.Threading;
using System.Threading.Tasks;

using Dapr.Actors.Runtime;

using Hexalith.Application.Tasks;

using Microsoft.Extensions.Logging;

/// <summary>
/// Class DaprRetryCallbackManager.
/// Implements the <see cref="IRetryCallbackManager" />.
/// </summary>
/// <seealso cref="IRetryCallbackManager" />
public class DaprRetryCallbackManager : IRetryCallbackManager
{
    /// <summary>
    /// The actor.
    /// </summary>
    private readonly Actor _actor;

    /// <summary>
    /// The logger.
    /// </summary>
    private readonly ILogger _logger;

    /// <summary>
    /// The reminder period.
    /// </summary>
    private readonly TimeSpan _reminderPeriod;

    /// <summary>
    /// The reminder TTL.
    /// </summary>
    private readonly TimeSpan _reminderTtl;

    /// <summary>
    /// The reminder exists.
    /// </summary>
    private bool? _reminderExists = null;

    /// <summary>
    /// The timer exists.
    /// </summary>
    private bool _timerExists = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="DaprRetryCallbackManager"/> class.
    /// </summary>
    /// <param name="actor">The actor.</param>
    /// <param name="reminderPeriod">The reminder period.</param>
    /// <param name="reminderTtl">The reminder TTL.</param>
    /// <param name="logger">The logger.</param>
    public DaprRetryCallbackManager(Actor actor, TimeSpan reminderPeriod, TimeSpan reminderTtl, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(actor);
        ArgumentNullException.ThrowIfNull(logger);
        _actor = actor;
        _reminderPeriod = reminderPeriod;
        _reminderTtl = reminderTtl;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task RegisterContinueCallbackAsync(
        TimeSpan dueTime,
        CancellationToken cancellationToken)
    {
        if (_timerExists)
        {
            await _actor
                .Host
                .TimerManager
                .UnregisterTimerAsync(new ActorTimerToken(
                        _actor.Host.ActorTypeInfo.ActorTypeName,
                        _actor.Id,
                        ActorConstants.ContinueTimerName));
        }

        await _actor
            .Host
            .TimerManager
            .RegisterTimerAsync(new ActorTimer(
                    _actor.Host.ActorTypeInfo.ActorTypeName,
                    _actor.Id,
                    ActorConstants.ContinueTimerName,
                    ActorConstants.ContinueCallbackMethodName,
                    [],
                    dueTime,
                    dueTime,
                    _reminderPeriod));
        _timerExists = true;

        if (await IsReminderRegisteredAsync())
        {
            ActorReminder newReminder = new(
                        _actor.Host.ActorTypeInfo.ActorTypeName,
                        _actor.Id,
                        ActorConstants.ContinueReminderName,
                        [],
                        _reminderPeriod,
                        _reminderPeriod,
                        _reminderTtl + _reminderPeriod);

            await _actor
                    .Host
                    .TimerManager
                    .RegisterReminderAsync(newReminder);
            _reminderExists = true;
        }
    }

    /// <inheritdoc/>
    public async Task UnregisterContinueCallbackAsync(CancellationToken cancellationToken)
    {
        if (_timerExists)
        {
            await _actor
                .Host
                .TimerManager
                .UnregisterTimerAsync(new ActorTimerToken(
                        _actor.Host.ActorTypeInfo.ActorTypeName,
                        _actor.Id,
                        ActorConstants.ContinueTimerName));
            _timerExists = false;
        }

        if (await IsReminderRegisteredAsync())
        {
            ActorReminder newReminder = new(
                        _actor.Host.ActorTypeInfo.ActorTypeName,
                        _actor.Id,
                        ActorConstants.ContinueReminderName,
                        [],
                        _reminderPeriod,
                        _reminderPeriod,
                        _reminderTtl + _reminderPeriod);

            await _actor
                    .Host
                    .TimerManager
                    .UnregisterReminderAsync(newReminder);
            _reminderExists = false;
        }
    }

    private async Task<bool> IsReminderRegisteredAsync()
    {
        if (_reminderExists != null)
        {
            return _reminderExists.Value;
        }

        IActorReminder? reminder;
        ActorReminderToken token = new(
                    _actor.Host.ActorTypeInfo.ActorTypeName,
                    _actor.Id,
                    ActorConstants.ContinueReminderName);

        try
        {
            reminder = await _actor
                .Host
                .TimerManager
                .GetReminderAsync(token);
        }
        catch (Exception e)
        {
            _logger.LogError(
                e,
                "Unable to get reminder '{ReminderName}' of actor '{ActorTypeName}' Id={ActorId}.",
                ActorConstants.ContinueReminderName,
                _actor.Host.ActorTypeInfo.ActorTypeName,
                _actor.Id.GetId());
            reminder = null;
        }

        return reminder != null;
    }
}