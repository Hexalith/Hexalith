// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Dynamics365Finance.Sales
// Author           : Jérôme Piquot
// Created          : 01-02-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 12-07-2023
// ***********************************************************************
// <copyright file="SalesInvoicePostedBusinessEvent.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Dynamics365Finance.Sales.SalesInvoices.IntegrationEvents;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Domain.Aggregates;
using Hexalith.Infrastructure.Dynamics365Finance.BusinessEvents;
using Hexalith.Infrastructure.Serialization.Serialization;

/// <summary>
/// This is the base class for logistics partner catalog events from Dynamics 365 for finance and operations.
/// </summary>
[DataContract]
public class SalesInvoicePostedBusinessEvent : Dynamics365BusinessEventBase
{
    /// <inheritdoc/>
    public override string AggregateId =>
        AggregateName + Aggregate.Separator +
        PartitionId + Aggregate.Separator +
        BusinessEventLegalEntity?.ToUpperInvariant() + Aggregate.Separator +
        OriginId + Aggregate.Separator +
        InvoiceId;

    /// <inheritdoc/>
    public override string AggregateName => nameof(Dynamics365Finance) + SalesInvoice.GetAggregateName();

    /// <summary>
    /// Gets or sets the invoice customer account.
    /// </summary>
    /// <value>The invoice account.</value>
    [DataMember]
    public string? InvoiceAccount { get; set; }

    /// <summary>
    /// Gets or sets the invoice amount in accounting currency.
    /// </summary>
    /// <value>The invoice amount in accounting currency.</value>
    [DataMember]
    public decimal InvoiceAmountInAccountingCurrency { get; set; }

    /// <summary>
    /// Gets or sets the invoice date.
    /// </summary>
    /// <value>The invoice date.</value>
    [DataMember]
    [JsonConverter(typeof(UnixEpochDateTimeConverter))]
    public DateTime? InvoiceDate { get; set; }

    /// <summary>
    /// Gets or sets the invoice due date.
    /// </summary>
    /// <value>The invoice due date.</value>
    [DataMember]
    [JsonConverter(typeof(UnixEpochDateTimeConverter))]
    public DateTime? InvoiceDueDate { get; set; }

    /// <summary>
    /// Gets or sets the invoice identifier.
    /// </summary>
    /// <value>The invoice identifier.</value>
    [DataMember]
    public string? InvoiceId { get; set; }

    /// <summary>
    /// Gets or sets the invoice tax amount.
    /// </summary>
    /// <value>The invoice tax amount.</value>
    [DataMember]
    public decimal InvoiceTaxAmount { get; set; }

    /// <summary>
    /// Gets or sets the first sales order identifier.
    /// </summary>
    /// <value>The sales order identifier.</value>
    [DataMember]
    public string? SalesOrderId { get; set; }
}