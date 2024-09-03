// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 04-25-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 04-25-2023
// ***********************************************************************
// <copyright file="NotificationSeverity.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.Notifications;

/// <summary>
/// Enum NotificationSeverity.
/// </summary>
public enum NotificationSeverity
{
    /// <summary>
    /// The information.
    /// </summary>
    Information,

    /// <summary>
    /// The warning.
    /// </summary>
    Warning,

    /// <summary>
    /// The error.
    /// </summary>
    Error,
}