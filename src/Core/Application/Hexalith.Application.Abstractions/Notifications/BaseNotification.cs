// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 02-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 04-25-2023
// ***********************************************************************
// <copyright file="BaseNotification.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Notifications;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Domain.Messages;
using Hexalith.Extensions.Serialization;

/// <summary>
/// Class BaseNotification.
/// Implements the <see cref="BaseMessage" />
/// Implements the <see cref="INotification" />.
/// </summary>
/// <seealso cref="BaseMessage" />
/// <seealso cref="INotification" />
[DataContract]
[JsonConverter(typeof(PolymorphicJsonConverter<BaseNotification>))]
public abstract class BaseNotification : BaseMessage, INotification
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseNotification" /> class.
    /// </summary>
    /// <param name="title">The title.</param>
    /// <param name="message">The message.</param>
    /// <param name="severity">The severity.</param>
    /// <param name="technicalDescription">The technical description.</param>
    [JsonConstructor]
    public BaseNotification(string title, string message, NotificationSeverity severity, string? technicalDescription)
    {
        Title = title;
        Message = message;
        Severity = severity;
        TechnicalDescription = technicalDescription;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseNotification" /> class.
    /// </summary>
    [Obsolete("For serialization only", true)]
    public BaseNotification() => Title = Message = string.Empty;

    /// <summary>
    /// Gets or sets the message.
    /// </summary>
    /// <value>The message.</value>
    public string Message { get; set; }

    /// <summary>
    /// Gets or sets the severity.
    /// </summary>
    /// <value>The severity.</value>
    public NotificationSeverity Severity { get; set; }

    /// <summary>
    /// Gets or sets the technical description.
    /// </summary>
    /// <value>The technical description.</value>
    public string? TechnicalDescription { get; set; }

    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    /// <value>The title.</value>
    public string Title { get; set; }
}