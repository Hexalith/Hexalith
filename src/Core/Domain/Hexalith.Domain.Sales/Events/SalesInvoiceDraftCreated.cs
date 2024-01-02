// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-02-2024
// ***********************************************************************
// <copyright file="SalesInvoiceDraftCreated.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.Events;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Extensions;

/// <summary>
/// Class SalesInvoiceRegistered.
/// Implements the <see cref="Hexalith.Domain.Events.SalesInvoiceEvent" />.
/// </summary>
/// <seealso cref="Hexalith.Domain.Events.SalesInvoiceEvent" />
[DataContract]
[Serializable]
public class SalesInvoiceDraftCreated : SalesInvoiceEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SalesInvoiceDraftCreated" /> class.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="originId">The origin identifier.</param>
    /// <param name="id">The identifier.</param>
    /// <param name="customerId">The customer identifier.</param>
    [JsonConstructor]
    public SalesInvoiceDraftCreated(
        string partitionId,
        string companyId,
        string originId,
        string id,
        string customerId)
        : base(partitionId, companyId, originId, id) => CustomerId = customerId;

    /// <summary>
    /// Initializes a new instance of the <see cref="SalesInvoiceDraftCreated" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    public SalesInvoiceDraftCreated() => CustomerId = string.Empty;

    /// <summary>
    /// Gets or sets the customer identifier.
    /// </summary>
    /// <value>The customer identifier.</value>
    [DataMember(Order = 10)]
    [JsonPropertyOrder(10)]
    public string CustomerId { get; set; }
}