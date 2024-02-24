// ***********************************************************************
// Assembly         : Hexalith.Domain.Sales
// Author           : Jérôme Piquot
// Created          : 01-02-2024
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-03-2024
// ***********************************************************************
// <copyright file="SalesInvoiceIssued.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.Events;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Domain.ValueObjets;
using Hexalith.Extensions;

/// <summary>
/// Class SalesInvoiceRegistered.
/// Implements the <see cref="Hexalith.Domain.Events.SalesInvoiceEvent" />.
/// </summary>
/// <seealso cref="Hexalith.Domain.Events.SalesInvoiceEvent" />
[DataContract]
[Serializable]
public class SalesInvoiceIssued : SalesInvoiceEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SalesInvoiceIssued" /> class.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="originId">The origin identifier.</param>
    /// <param name="id">The identifier.</param>
    /// <param name="createdDate">The created date.</param>
    /// <param name="customerId">The customer identifier.</param>
    /// <param name="currencyId">The currency identifier.</param>
    /// <param name="lines">The lines.</param>
    [JsonConstructor]
    public SalesInvoiceIssued(
        string partitionId,
        string companyId,
        string originId,
        string id,
        DateTimeOffset createdDate,
        string customerId,
        string currencyId,
        IEnumerable<SalesInvoiceLine> lines)
        : base(partitionId, companyId, originId, id)
    {
        CreatedDate = createdDate;
        CustomerId = customerId;
        CurrencyId = currencyId;
        Lines = lines.Select(p => new SalesInvoiceLine(p)).ToList();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SalesInvoiceIssued" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    public SalesInvoiceIssued()
    {
        CustomerId = CurrencyId = string.Empty;
        Lines = [];
    }

    /// <summary>
    /// Gets or sets the created date.
    /// </summary>
    /// <value>The created date.</value>
    [DataMember(Order = 10)]
    [JsonPropertyOrder(10)]
    public DateTimeOffset CreatedDate { get; set; }

    /// <summary>
    /// Gets or sets the currency identifier.
    /// </summary>
    /// <value>The currency identifier.</value>
    [DataMember(Order = 12)]
    [JsonPropertyOrder(12)]
    public string CurrencyId { get; set; }

    /// <summary>
    /// Gets or sets the customer identifier.
    /// </summary>
    /// <value>The customer identifier.</value>
    [DataMember(Order = 11)]
    [JsonPropertyOrder(11)]
    public string CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the lines.
    /// </summary>
    /// <value>The lines.</value>
    [DataMember(Order = 20)]
    [JsonPropertyOrder(20)]
    public IEnumerable<SalesInvoiceLine> Lines { get; set; }
}