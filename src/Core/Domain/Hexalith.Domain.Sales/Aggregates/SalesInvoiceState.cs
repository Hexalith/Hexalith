// ***********************************************************************
// Assembly         : Hexalith.Domain.Sales
// Author           : Jérôme Piquot
// Created          : 01-02-2024
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-02-2024
// ***********************************************************************
// <copyright file="SalesInvoiceState.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.Aggregates;

using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Domain.Entities;
using Hexalith.Domain.Events;
using Hexalith.Domain.Organizations.Aggregates;

/// <summary>
/// Class SalesInvoiceState.
/// Implements the <see cref="EntityAggregateState" />.
/// </summary>
/// <seealso cref="EntityAggregateState" />
[DataContract]
[Serializable]
public class SalesInvoiceState : EntityAggregateState
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SalesInvoiceState"/> class.
    /// </summary>
    public SalesInvoiceState()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SalesInvoiceState"/> class.
    /// </summary>
    /// <param name="issued">The issued.</param>
    public SalesInvoiceState(SalesInvoiceIssued issued)
    {
    }

    /// <summary>
    /// Gets or sets the created date.
    /// </summary>
    /// <value>The created date.</value>
    [DataMember(Order = 10)]
    [JsonPropertyOrder(10)]
    public DateTimeOffset? CreatedDate { get; set; }

    /// <summary>
    /// Gets or sets the currency identifier.
    /// </summary>
    /// <value>The currency identifier.</value>
    [DataMember(Order = 12)]
    [JsonPropertyOrder(12)]
    public string? CurrencyId { get; set; }

    /// <summary>
    /// Gets or sets the customer identifier.
    /// </summary>
    /// <value>The customer identifier.</value>
    [DataMember(Order = 11)]
    [JsonPropertyOrder(11)]
    public string? CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the lines.
    /// </summary>
    /// <value>The lines.</value>
    [DataMember(Order = 20)]
    [JsonPropertyOrder(20)]
    public IEnumerable<SalesInvoiceLineState> Lines { get; set; } = new List<SalesInvoiceLineState>();
}