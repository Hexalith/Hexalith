// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 02-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-26-2023
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
[Serializable]
public class BaseNotification : BaseMessage, INotification
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseNotification" /> class.
    /// </summary>
    /// <param name="correlationId">The correlation identifier.</param>
    /// <param name="sourceAggregateId">The source aggregate identifier.</param>
    /// <param name="sourceAggregateName">Name of the source aggregate.</param>
    /// <param name="title">The title.</param>
    /// <param name="message">The message.</param>
    /// <param name="severity">The severity.</param>
    /// <param name="technicalDescription">The technical description.</param>
    [JsonConstructor]
    public BaseNotification(
        string correlationId,
        string sourceAggregateName,
        string sourceAggregateId,
        string title,
        string message,
        NotificationSeverity severity,
        string? technicalDescription)
    {
        Title = title;
        Message = message;
        Severity = severity;
        TechnicalDescription = technicalDescription;
        CorrelationId = correlationId;
        SourceAggregateId = sourceAggregateId;
        SourceAggregateName = sourceAggregateName;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseNotification" /> class.
    /// </summary>
    [Obsolete("For serialization only", true)]
    public BaseNotification() => CorrelationId = SourceAggregateId = SourceAggregateName = Title = Message = string.Empty;

    /// <summary>
    /// Gets or sets the correlation identifier.
    /// </summary>
    /// <value>The correlation identifier.</value>
    [JsonPropertyOrder(2)]
    [DataMember(Order = 2)]
    public string CorrelationId { get; set; }

    /// <summary>
    /// Gets or sets the message.
    /// </summary>
    /// <value>The message.</value>
    [JsonPropertyOrder(6)]
    [DataMember(Order = 6)]
    public string Message { get; set; }

    /// <summary>
    /// Gets or sets the severity.
    /// </summary>
    /// <value>The severity.</value>
    [JsonPropertyOrder(1)]
    [DataMember(Order = 1)]
    public NotificationSeverity Severity { get; set; }

    /// <summary>
    /// Gets or sets the source aggregate identifier.
    /// </summary>
    /// <value>The source aggregate identifier.</value>
    [JsonPropertyOrder(4)]
    [DataMember(Order = 4)]
    public string? SourceAggregateId { get; set; }

    /// <summary>
    /// Gets or sets the name of the source aggregate.
    /// </summary>
    /// <value>The name of the source aggregate.</value>
    [JsonPropertyOrder(3)]
    [DataMember(Order = 3)]
    public string? SourceAggregateName { get; set; }

    /// <summary>
    /// Gets or sets the technical description.
    /// </summary>
    /// <value>The technical description.</value>
    [JsonPropertyOrder(7)]
    [DataMember(Order = 7)]
    public string? TechnicalDescription { get; set; }

    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    /// <value>The title.</value>
    [JsonPropertyOrder(5)]
    [DataMember(Order = 5)]
    public string Title { get; set; }

    /// <inheritdoc/>
    protected override string DefaultAggregateId()
        => SourceAggregateId ?? string.Empty;

    /// <inheritdoc/>
    protected override string DefaultAggregateName()
        => string.IsNullOrWhiteSpace(SourceAggregateName) ? ApplicationConstants.NotificationDefaultAggregateName : SourceAggregateName;
}