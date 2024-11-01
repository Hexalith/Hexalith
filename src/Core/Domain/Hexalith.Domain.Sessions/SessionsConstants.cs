// <copyright file="SessionsConstants.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Sessions;

/// <summary>
/// Provides constant values used in the sessions domain.
/// </summary>
public class SessionsConstants
{
    /// <summary>
    /// Gets the name of the session aggregate.
    /// </summary>
    /// <value>The string "Session".</value>
    public static string SessionAggregateName => "Session";

    /// <summary>
    /// Gets the name of the user aggregate.
    /// </summary>
    /// <value>The string "User".</value>
    public static string UserAggregateName => "User";
}
