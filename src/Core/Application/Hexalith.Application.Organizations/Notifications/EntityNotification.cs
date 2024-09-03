// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 10-16-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 11-21-2023
// ***********************************************************************
// <copyright file="EntityNotification.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Organizations.Notifications;

using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Domain.Notifications;
using Hexalith.Extensions;

/// <summary>
/// Class EntityNotification.
/// Implements the <see cref="Domain.Notifications.PartitionedNotification" />.
/// </summary>
/// <seealso cref="Domain.Notifications.PartitionedNotification" />
[DataContract]
[Serializable]
public abstract class EntityNotification : PartitionedNotification
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EntityNotification" /> class.
    /// </summary>
    /// <param name="originId">The origin identifier.</param>
    /// <param name="id">The identifier.</param>
    /// <param name="correlationId">The correlation identifier.</param>
    /// <param name="sourceAggregateName">Name of the source aggregate.</param>
    /// <param name="sourceAggregateId">The source aggregate identifier.</param>
    /// <param name="title">The title.</param>
    /// <param name="message">The message.</param>
    /// <param name="severity">The severity.</param>
    /// <param name="technicalDescription">The technical description.</param>
    [JsonConstructor]
    protected EntityNotification(
        string originId,
        string id,
        string correlationId,
        string sourceAggregateName,
        string sourceAggregateId,
        string title,
        string message,
        NotificationSeverity severity,
        string? technicalDescription)
        : base(
            correlationId,
            sourceAggregateName,
            sourceAggregateId,
            title,
            message,
            severity,
            technicalDescription)
    {
        OriginId = originId;
        Id = id;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityNotification" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    protected EntityNotification() => OriginId = Id = string.Empty;

    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    [DataMember(Order = 3)]
    [JsonPropertyOrder(3)]
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the company identifier.
    /// </summary>
    /// <value>The company identifier.</value>
    [DataMember(Order = 2)]
    [JsonPropertyOrder(2)]
    public string OriginId { get; set; }

    /// <inheritdoc/>
    protected override string DefaultAggregateId()
        => base.DefaultAggregateId() + Separator + OriginId + Separator + Id;
}