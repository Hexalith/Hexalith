// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 10-26-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-26-2023
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
    /// The event bus name.
    /// </summary>
    public const string EventBus = "event-bus";

    /// <summary>
    /// The notification bus name.
    /// </summary>
    public const string NotificationBus = "notification-bus";

    /// <summary>
    /// The request bus name.
    /// </summary>
    public const string RequestBus = "request-bus";

    /// <summary>
    /// The system user name.
    /// </summary>
    public const string SystemUser = "system";
}