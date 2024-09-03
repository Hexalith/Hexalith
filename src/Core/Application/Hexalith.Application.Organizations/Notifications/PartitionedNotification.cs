// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 10-16-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-16-2023
// ***********************************************************************
// <copyright file="PartitionedNotification.cs" company="Jérôme Piquot">
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
/// Class PartitionedNotification.
/// Implements the <see cref="BaseNotification" />.
/// </summary>
/// <seealso cref="BaseNotification" />
[DataContract]
[Serializable]
public abstract class PartitionedNotification : BaseNotification
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PartitionedNotification" /> class.
    /// </summary>
    /// <param name="correlationId">The correlation identifier.</param>
    /// <param name="sourceAggregateName">Name of the source aggregate.</param>
    /// <param name="sourceAggregateId">The source aggregate identifier.</param>
    /// <param name="title">The title.</param>
    /// <param name="message">The message.</param>
    /// <param name="severity">The severity.</param>
    /// <param name="technicalDescription">The technical description.</param>
    [JsonConstructor]
    protected PartitionedNotification(
        string correlationId,
        string sourceAggregateName,
        string sourceAggregateId,
        string title,
        string message,
        NotificationSeverity severity,
        string? technicalDescription)
        : base(
            sourceAggregateName,
            sourceAggregateId,
            title,
            message,
            severity,
            technicalDescription)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PartitionedNotification" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    protected PartitionedNotification() => PartitionId = string.Empty;

    /// <summary>
    /// Gets or sets the partition identifier.
    /// </summary>
    /// <value>The partition identifier.</value>
    [DataMember(Order = 1)]
    [JsonPropertyOrder(1)]
    public string PartitionId { get; set; }

    /// <inheritdoc/>
    protected override string DefaultAggregateId()
        => DefaultAggregateName() + Separator + PartitionId;
}