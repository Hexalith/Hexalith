// ***********************************************************************
// Assembly         : Christofle.Infrastructure.Dynamics365Finance.RestClient
// Author           : Jérôme Piquot
// Created          : 02-12-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-12-2023
// ***********************************************************************
// <copyright file="SalesOrderHeaderChargeCreate.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Dynamics365Finance.Sales.SalesOrders.Models;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

/// <summary>
/// Class SalesOrderHeaderChargeCreate.
/// Implements the <see cref="IEquatable{SalesOrderHeaderChargeCreate}" />.
/// </summary>
/// <seealso cref="IEquatable{SalesOrderHeaderChargeCreate}" />
[DataContract]
public record SalesOrderHeaderChargeCreate
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SalesOrderHeaderChargeCreate" /> class.
    /// </summary>
    /// <param name="dataAreaId">The data area identifier.</param>
    /// <param name="salesOrderNumber">The sales order number.</param>
    /// <param name="chargeLineNumber">The charge line number.</param>
    /// <param name="chargeDescription">The charge description.</param>
    /// <param name="salesChargeCode">The sales charge code.</param>
    /// <param name="fixedChargeAmount">The fixed charge amount.</param>
    public SalesOrderHeaderChargeCreate(
        string dataAreaId,
        string salesOrderNumber,
        decimal chargeLineNumber,
        string chargeDescription,
        string salesChargeCode,
        decimal fixedChargeAmount)
    {
        DataAreaId = dataAreaId;
        SalesOrderNumber = salesOrderNumber;
        ChargeLineNumber = chargeLineNumber;
        ChargeDescription = chargeDescription;
        SalesChargeCode = salesChargeCode;
        FixedChargeAmount = fixedChargeAmount;
    }

    /// <summary>
    /// Gets the data area identifier.
    /// </summary>
    /// <value>The data area identifier.</value>
    [JsonPropertyName("dataAreaId")]
    public string DataAreaId { get; }

    /// <summary>
    /// Gets the sales order number.
    /// </summary>
    /// <value>The sales order number.</value>
    public string SalesOrderNumber { get; }

    /// <summary>
    /// Gets the charge line number.
    /// </summary>
    /// <value>The charge line number.</value>
    public decimal ChargeLineNumber { get; }

    /// <summary>
    /// Gets the charge description.
    /// </summary>
    /// <value>The charge description.</value>
    public string ChargeDescription { get; }

    /// <summary>
    /// Gets the sales charge code.
    /// </summary>
    /// <value>The sales charge code.</value>
    public string SalesChargeCode { get; }

    /// <summary>
    /// Gets the fixed charge amount.
    /// </summary>
    /// <value>The fixed charge amount.</value>
    public decimal FixedChargeAmount { get; }
}