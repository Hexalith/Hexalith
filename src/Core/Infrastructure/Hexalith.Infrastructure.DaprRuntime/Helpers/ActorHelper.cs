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
#pragma warning disable CA1055 // URI-like return values should not be strings

    public static string ToUrlDecodedString(this ActorId actorId)
#pragma warning restore CA1055 // URI-like return values should not be strings
    {
        ArgumentNullException.ThrowIfNull(actorId);
        return HttpUtility.UrlDecode(actorId.GetId()).Replace("~", " ", StringComparison.InvariantCultureIgnoreCase);
    }

    /// <summary>
    /// Converts to an actor id with an url encoded identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>ActorId.</returns>
    public static ActorId ToUrlEncodedActorId(this string id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);
        return id.Contains('~', StringComparison.InvariantCultureIgnoreCase)
            ? throw new ArgumentException($"The '~' character is not supported.")
            : new ActorId(HttpUtility.UrlEncode(id.Replace(" ", "~", StringComparison.InvariantCultureIgnoreCase)));
    }
}