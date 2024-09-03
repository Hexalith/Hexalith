// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 02-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 04-25-2023
// ***********************************************************************
// <copyright file="INotification.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.Notifications;

using Hexalith.Domain.Messages;

/// <summary>
/// Interface for all commands.
/// </summary>
public interface INotification : IMessage
{
    /// <summary>
    /// Gets the message.
    /// </summary>
    /// <value>The message.</value>
    public string Message { get; }

    /// <summary>
    /// Gets the severity.
    /// </summary>
    /// <value>The severity.</value>
    public NotificationSeverity Severity { get; }

    /// <summary>
    /// Gets the technical description.
    /// </summary>
    /// <value>The technical description.</value>
    public string? TechnicalDescription { get; }

    /// <summary>
    /// Gets the title.
    /// </summary>
    /// <value>The title.</value>
    public string Title { get; }
}