// <copyright file="ApplicationConstants.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application;

/// <summary>
/// Application constants.
/// </summary>
public static class ApplicationConstants
{
    /// <summary>
    /// Gets the command bus name.
    /// </summary>
    public static string CommandBus => "commands";

    /// <summary>
    /// Gets the command bus suffix.
    /// </summary>
    public static string CommandBusSuffix => "-commands";

    /// <summary>
    /// Gets the event bus name.
    /// </summary>
    public static string EventBus => "events";

    /// <summary>
    /// Gets the event bus suffix.
    /// </summary>
    public static string EventBusSuffix => "-events";

    /// <summary>
    /// Gets the identifier part separator.
    /// </summary>
    public static string IdPartSeparator => "-";

    /// <summary>
    /// Gets the notification bus name.
    /// </summary>
    public static string NotificationBus => "notifications";

    /// <summary>
    /// Gets the notification bus suffixe.
    /// </summary>
    public static string NotificationBusSuffix => "-notifications";

    /// <summary>
    /// Gets the notification default aggregate name.
    /// </summary>
    public static string NotificationDefaultAggregateName => "Global";

    /// <summary>
    /// Gets the request bus name.
    /// </summary>
    public static string RequestBus => "requests";

    /// <summary>
    /// Gets the request bus suffixe.
    /// </summary>
    public static string RequestBusSuffix => "-requests";

    /// <summary>
    /// Gets the state name.
    /// </summary>
    public static string StateName => "state";

    /// <summary>
    /// Gets the system user name.
    /// </summary>
    public static string SystemUser => "system";

    /// <summary>
    /// Gets the require sessions.
    /// </summary>
    public static string TopicRequireSessions => "requireSessions=true";

    /// <summary>
    /// Gets the user defined culture property name.
    /// </summary>
    public static string UserDefinedCulturePropertyName => "hexalithCulture";
}