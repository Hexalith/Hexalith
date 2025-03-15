// <copyright file="SessionConstants.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Sessions;

using Hexalith.Application.Sessions.Models;

/// <summary>
/// Provides constant values for session properties.
/// </summary>
public static class SessionConstants
{
    /// <summary>
    /// Gets the name of the session expiration property.
    /// </summary>
    public static string SessionExpirationName => $"{nameof(Session)}{nameof(Session.Expiration)}";

    /// <summary>
    /// Gets the name of the session ID property.
    /// </summary>
    public static string SessionIdName => $"{nameof(Session)}{nameof(Session.Id)}";
}