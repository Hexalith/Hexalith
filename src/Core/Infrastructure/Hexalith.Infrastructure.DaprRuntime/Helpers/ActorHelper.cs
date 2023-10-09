// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprRuntime
// Author           : Jérôme Piquot
// Created          : 10-03-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-04-2023
// ***********************************************************************
// <copyright file="ActorHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.DaprRuntime.Helpers;

using System;
using System.Web;

using Dapr.Actors;
using Dapr.Actors.Runtime;

using Microsoft.Extensions.Logging;

/// <summary>
/// Class ActorHelper.
/// </summary>
public static class ActorHelper
{
    /// <summary>
    /// Register continue callback reminder as an asynchronous operation.
    /// </summary>
    /// <param name="actor">The actor.</param>
    /// <param name="dueTime">The due time.</param>
    /// <param name="reminderPeriod">The reminder period.</param>
    /// <param name="ttl">The TTL.</param>
    /// <param name="timerExist">The timer exist.</param>
    /// <param name="logger">The logger.</param>
    /// <returns>A Task&lt;(Dapr.Actors.Runtime.IActorReminder Reminder, Dapr.Actors.Runtime.ActorTimer Timer)&gt; representing the asynchronous operation.</returns>
    public static async Task<(IActorReminder Reminder, ActorTimer Timer)> RegisterContinueCallbackReminderAsync(
            this Actor actor,
            TimeSpan dueTime,
            TimeSpan reminderPeriod,
            TimeSpan ttl,
            bool timerExist,
            ILogger logger)
    {
        ActorTimer timer = new(
                    actor.Host.ActorTypeInfo.ActorTypeName,
                    actor.Id,
                    ActorConstants.ContinueTimerName,
                    ActorConstants.ContinueCallbackMethodName,
                    [],
                    dueTime,
                    dueTime,
                    reminderPeriod);
        await actor
            .Host
            .TimerManager
            .RegisterTimerAsync(timer);

        IActorReminder? reminder;
        ActorReminderToken token = new(
                    actor.Host.ActorTypeInfo.ActorTypeName,
                    actor.Id,
                    ActorConstants.ContinueReminderName);
        try
        {
            reminder = await actor
                .Host
                .TimerManager
                .GetReminderAsync(token);
        }
        catch (Exception e)
        {
            logger.LogError(
                e,
                "Unable to get reminder '{ReminderName}' of actor '{ActorTypeName}' Id={ActorId}.",
                ActorConstants.ContinueReminderName,
                actor.Host.ActorTypeInfo.ActorTypeName,
                actor.Id.GetId());
            reminder = null;
        }

        if (reminder == null)
        {
            ActorReminder newReminder = new(
                        actor.Host.ActorTypeInfo.ActorTypeName,
                        actor.Id,
                        ActorConstants.ContinueReminderName,
                        [],
                        reminderPeriod,
                        reminderPeriod,
                        ttl + reminderPeriod);

            await actor
                    .Host
                    .TimerManager
                    .RegisterReminderAsync(newReminder);
            reminder = newReminder;
        }

        if (timerExist)
        {
            await actor
                .Host
                .TimerManager
                .UnregisterTimerAsync(new ActorTimerToken(
                        actor.Host.ActorTypeInfo.ActorTypeName,
                        actor.Id,
                        ActorConstants.ContinueTimerName));
        }

        return (reminder, timer);
    }

    /// <summary>
    /// Converts to decoded string.
    /// </summary>
    /// <param name="actorId">The actor identifier.</param>
    /// <returns>System.String.</returns>
    /// <exception cref="System.ArgumentNullException">Null argument.</exception>
    public static string ToUrlDecodedString(this ActorId actorId)
    {
        ArgumentNullException.ThrowIfNull(actorId);
        return HttpUtility.UrlDecode(actorId.GetId()).Replace("~", " ");
    }

    /// <summary>
    /// Converts to an actor id with an url encoded identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>ActorId.</returns>
    public static ActorId ToUrlEncodedActorId(this string id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);
        return id.Contains('~')
            ? throw new ArgumentException($"The '~' character is not supported.")
            : new ActorId(HttpUtility.UrlEncode(id.Replace(" ", "~")));
    }

    /// <summary>
    /// Unregister continue callback reminder as an asynchronous operation.
    /// </summary>
    /// <param name="actor">The actor.</param>
    /// <param name="removeTimer">if set to <c>true</c> [remove timer].</param>
    /// <param name="getReminder">The get reminder.</param>
    /// <param name="unregisterReminder">The unregister reminder.</param>
    /// <param name="unregisterTimer">The unregister timer.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public static async Task UnregisterContinueCallbackReminderAsync(
            this Actor actor,
            bool removeTimer,
            Func<string, Task<IActorReminder>> getReminder,
            Func<string, Task> unregisterReminder,
            Func<string, Task> unregisterTimer)
    {
        if (await getReminder(ActorConstants.ContinueReminderName) != null)
        {
            await unregisterReminder(ActorConstants.ContinueTimerName);
        }

        if (removeTimer)
        {
            await unregisterTimer(ActorConstants.ContinueTimerName);
        }
    }
}