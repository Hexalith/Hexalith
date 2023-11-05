// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 10-26-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 11-04-2023
// ***********************************************************************
// <copyright file="ApplicationConstants.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application;

/// <summary>
/// Class ApplicationConstants.
/// </summary>
public static class ApplicationConstants
{
    /// <summary>
    /// The command bus name.
    /// </summary>
    public const string CommandBus = "command-bus";

    /// <summary>
    /// The command bus suffixe.
    /// </summary>
    public const string CommandBusSuffix = "-commands";

    /// <summary>
    /// The event bus name.
    /// </summary>
    public const string EventBus = "event-bus";

    /// <summary>
    /// The event bus suffixe.
    /// </summary>
    public const string EventBusSuffix = "-events";

    /// <summary>
    /// The identifier part separator.
    /// </summary>
    public const string IdPartSeparator = "-";

    /// <summary>
    /// The notification bus name.
    /// </summary>
    public const string NotificationBus = "notification-bus";

    /// <summary>
    /// The notification bus suffixe.
    /// </summary>
    public const string NotificationBusSuffix = "-notifications";

    /// <summary>
    /// The notification default aggregate name.
    /// </summary>
    public const string NotificationDefaultAggregateName = "Global";

    /// <summary>
    /// The request bus name.
    /// </summary>
    public const string RequestBus = "request-bus";

    /// <summary>
    /// The request bus suffixe.
    /// </summary>
    public const string RequestBusSuffix = "-requests";

    /// <summary>
    /// The state name.
    /// </summary>
    public const string StateName = "state";

    /// <summary>
    /// The system user name.
    /// </summary>
    public const string SystemUser = "system";

    /// <summary>
    /// The require sessions.
    /// </summary>
    public const string TopicRequireSessions = "requireSessions: true";
}