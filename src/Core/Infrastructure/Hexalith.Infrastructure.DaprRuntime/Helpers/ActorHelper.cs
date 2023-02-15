// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprRuntime
// Author           : jpiquot
// Created          : 02-15-2023
//
// Last Modified By : jpiquot
// Last Modified On : 02-15-2023
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
    /// Converts to an actor id with an url encoded identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>ActorId.</returns>
    public static ActorId ToUrlEncodedActorId(this string id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);
        return new ActorId(HttpUtility.UrlEncode(id).Replace("+", "%20"));
    }
}
