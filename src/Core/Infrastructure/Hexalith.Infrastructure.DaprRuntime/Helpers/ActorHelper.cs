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

/// <summary>
/// Class ActorHelper.
/// </summary>
public static class ActorHelper
{
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