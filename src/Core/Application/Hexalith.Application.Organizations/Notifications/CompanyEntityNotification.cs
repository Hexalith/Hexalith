// ***********************************************************************
// Assembly         : Hexalith.Domain.Abstractions
// Author           : Jérôme Piquot
// Created          : 10-16-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-16-2023
// ***********************************************************************
// <copyright file="CompanyEntityNotification.cs" company="Jérôme Piquot">
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
/// Class CompanyEntityNotification.
/// Implements the <see cref="Domain.Notifications.CompanyNotification" />.
/// </summary>
/// <seealso cref="Domain.Notifications.CompanyNotification" />
[DataContract]
[Serializable]
public abstract class CompanyEntityNotification : EntityNotification
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CompanyEntityNotification" /> class.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
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
    protected CompanyEntityNotification(
        string companyId,
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
            originId,
            id,
            correlationId,
            sourceAggregateName,
            sourceAggregateId,
            title,
            message,
            severity,
            technicalDescription)
            => CompanyId = companyId;

    /// <summary>
    /// Initializes a new instance of the <see cref="CompanyEntityNotification" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    protected CompanyEntityNotification() => CompanyId = string.Empty;

    /// <summary>
    /// Gets or sets the company identifier.
    /// </summary>
    /// <value>The company identifier.</value>
    [DataMember(Order = 4)]
    [JsonPropertyOrder(4)]
    public string CompanyId { get; set; }

    /// <inheritdoc/>
    protected override string DefaultAggregateId()
        => DefaultAggregateName() + Separator + PartitionId + Separator + CompanyId + Separator + OriginId + Separator + Id;
}