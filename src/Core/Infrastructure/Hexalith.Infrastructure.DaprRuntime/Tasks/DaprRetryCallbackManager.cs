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
public partial class DaprRetryCallbackManager : IRetryCallbackManager
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
    private readonly TimeSpan? _reminderTtl;

    /// <summary>
    /// The reminder exists.
    /// </summary>
    private IActorReminder? _reminder;

    /// <summary>
    /// The timer exists.
    /// </summary>
    private ActorTimer? _timer;

    /// <summary>
    /// Initializes a new instance of the <see cref="DaprRetryCallbackManager"/> class.
    /// </summary>
    /// <param name="actor">The actor.</param>
    /// <param name="reminderPeriod">The reminder period.</param>
    /// <param name="reminderTtl">The reminder TTL.</param>
    /// <param name="logger">The logger.</param>
    public DaprRetryCallbackManager(Actor actor, TimeSpan reminderPeriod, TimeSpan? reminderTtl, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(actor);
        ArgumentNullException.ThrowIfNull(logger);
        _actor = actor;
        _reminderPeriod = reminderPeriod < TimeSpan.FromMinutes(1) ? TimeSpan.FromMinutes(1) : reminderPeriod;
        _reminderTtl = reminderTtl;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task RegisterContinueCallbackAsync(
        TimeSpan dueTime,
        CancellationToken cancellationToken)
    {
        RegisterContinueCallbackInformation(_actor.Id.GetId(), dueTime);
        await RegisterTimerAsync(dueTime, cancellationToken).ConfigureAwait(false);
        if (await IsReminderRegisteredAsync().ConfigureAwait(false) == null)
        {
            ActorReminder newReminder = (_reminderTtl == null)
                ? new(
                        _actor.Host.ActorTypeInfo.ActorTypeName,
                        _actor.Id,
                        ActorConstants.ContinueReminderName,
                        [],
                        _reminderPeriod,
                        _reminderPeriod)
             : new(
                        _actor.Host.ActorTypeInfo.ActorTypeName,
                        _actor.Id,
                        ActorConstants.ContinueReminderName,
                        [],
                        _reminderPeriod > dueTime ? _reminderPeriod : dueTime,
                        _reminderPeriod,
                        _reminderTtl.Value);

            await _actor
                    .Host
                    .TimerManager
                    .RegisterReminderAsync(newReminder).ConfigureAwait(false);
            _reminder = newReminder;
        }
    }

    /// <inheritdoc/>
    public async Task UnregisterContinueCallbackAsync(CancellationToken cancellationToken)
    {
        UnregisterContinueCallbackInformation(_actor.Id.GetId());
        if (_timer != null)
        {
            await _actor
                .Host
                .TimerManager
                .UnregisterTimerAsync(new ActorTimerToken(
                        _timer.ActorType,
                        _timer.ActorId,
                        _timer.Name)).ConfigureAwait(false);
            ContinueCallbackTimerUnregisteredInformation(_timer.ActorId.GetId(), _timer.DueTime, _timer.Period, _timer.Ttl);
            _timer = null;
        }

        if (await IsReminderRegisteredAsync().ConfigureAwait(false) != null)
        {
            await _actor
                    .Host
                    .TimerManager
                    .UnregisterReminderAsync(new ActorReminderToken(
                        _actor.Host.ActorTypeInfo.ActorTypeName,
                        _actor.Id,
                        _reminder?.Name))
                    .ConfigureAwait(false);
            _reminder = null;
        }
    }

    [LoggerMessage(
           EventId = 3,
           Level = LogLevel.Information,
           Message = "Timer for {AggregateId} registered. Due time : {DueTime}; Period : {Period}; Timeout : {TimeOut}.",
           EventName = nameof(DaprRetryCallbackManager))]
    private partial void ContinueCallbackTimerRegisteredInformation(string aggregateId, TimeSpan dueTime, TimeSpan period, TimeSpan? timeout);

    [LoggerMessage(
            EventId = 4,
            Level = LogLevel.Information,
            Message = "Timer for {AggregateId} unregistered. Due time : {DueTime}; Period : {Period}; Timeout : {TimeOut}.",
            EventName = nameof(DaprRetryCallbackManager))]
    private partial void ContinueCallbackTimerUnregisteredInformation(string aggregateId, TimeSpan dueTime, TimeSpan period, TimeSpan? timeout);

    [LoggerMessage(
            EventId = 5,
            Level = LogLevel.Error,
            Message = "Error while getting reminder {ReminderName} for {AggregateName} with Id '{AggregateId}'.",
            EventName = nameof(DaprRetryCallbackManager))]
    private partial void GetReminderError(string aggregateName, string aggregateId, string reminderName, Exception exception);

    private async Task<IActorReminder?> IsReminderRegisteredAsync()
    {
        if (_reminder != null)
        {
            return _reminder;
        }

        ActorReminderToken token = new(
                    _actor.Host.ActorTypeInfo.ActorTypeName,
                    _actor.Id,
                    ActorConstants.ContinueReminderName);

        try
        {
            _reminder = await _actor
                .Host
                .TimerManager
                .GetReminderAsync(token).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            GetReminderError(token.ActorType, token.ActorId.GetId(), token.Name, ex);
            _reminder = null;
            throw;
        }

        return _reminder;
    }

    [LoggerMessage(
           EventId = 1,
           Level = LogLevel.Information,
           Message = "Registering continue call back in {DueTime} for {AggregateId}.",
           EventName = nameof(DaprRetryCallbackManager))]
    private partial void RegisterContinueCallbackInformation(string aggregateId, TimeSpan dueTime);

    private async Task RegisterTimerAsync(
                    TimeSpan dueTime,
                    CancellationToken cancellationToken)
    {
        if (_timer != null)
        {
            await _actor
                .Host
                .TimerManager
                .UnregisterTimerAsync(new ActorTimerToken(
                        _actor.Host.ActorTypeInfo.ActorTypeName,
                        _actor.Id,
                        ActorConstants.ContinueTimerName)).ConfigureAwait(false);
            ContinueCallbackTimerUnregisteredInformation(_timer.ActorId.GetId(), _timer.DueTime, _timer.Period, _timer.Ttl);
            _timer = null;
        }

        if (dueTime < _reminderPeriod)
        {
            ActorTimer timer = (_reminderTtl > TimeSpan.Zero)
            ? new ActorTimer(
            _actor.Host.ActorTypeInfo.ActorTypeName,
            _actor.Id,
            ActorConstants.ContinueTimerName,
            ActorConstants.ContinueCallbackMethodName,
            [],
            dueTime,
            dueTime,
            _reminderPeriod)
                : new ActorTimer(
                        _actor.Host.ActorTypeInfo.ActorTypeName,
                        _actor.Id,
                        ActorConstants.ContinueTimerName,
                        ActorConstants.ContinueCallbackMethodName,
                        [],
                        dueTime,
                        dueTime);

            await _actor
                .Host
                .TimerManager
                .RegisterTimerAsync(timer).ConfigureAwait(false);
            _timer = timer;
            ContinueCallbackTimerRegisteredInformation(timer.ActorId.GetId(), timer.DueTime, timer.Period, timer.Ttl);
        }
    }

    [LoggerMessage(
          EventId = 2,
          Level = LogLevel.Information,
          Message = "Unregistering continue call back for {AggregateId}.",
          EventName = nameof(DaprRetryCallbackManager))]
    private partial void UnregisterContinueCallbackInformation(string aggregateId);
}