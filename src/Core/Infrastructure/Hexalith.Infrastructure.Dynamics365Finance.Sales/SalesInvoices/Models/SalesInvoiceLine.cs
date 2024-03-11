// ***********************************************************************
// Assembly         : Christofle.Infrastructure.Dynamics365Finance.RestClient
// Author           : Jérôme Piquot
// Created          : 01-13-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 03-09-2023
// ***********************************************************************
// <copyright file="SalesInvoiceLine.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Dynamics365Finance.Sales.SalesInvoices.Models;

using System.Runtime.Serialization;

using Hexalith.Infrastructure.Dynamics365Finance.Models;

/// <summary>
/// Class SalesOrderLine.
/// Implements the <see cref="ODataElement" />
/// Implements the <see cref="IEquatable{ODataElement}" />
/// Implements the <see cref="IODataElement" />
/// Implements the <see cref="IEquatable{SalesOrderLine}" />.
/// </summary>
/// <seealso cref="ODataElement" />
/// <seealso cref="IEquatable{ODataElement}" />
/// <seealso cref="IODataElement" />
/// <seealso cref="IEquatable{SalesOrderLine}" />
[DataContract]
public record SalesInvoiceLine(
    string Etag,
    string DataAreaId,
    string InvoiceNumber,
    int LineCreationSequenceNumber,
    DateTimeOffset InvoiceDate,
    decimal InvoicedQuantity,
    string ProductName,
    decimal SalesPrice,
    decimal LineTotalTaxAmount,
    decimal LineAmount,
    string CurrencyCode,
    string SalesUnitSymbol,
    string ProductNumber,
    string? ProductSizeId,
    string? ProductColorId,
    string? ProductVersionId,
    string? ProductStyleId,
    decimal? LineTotalChargeAmount,
    decimal? LineTotalDiscountAmount,
    string? InventorySiteId,
    string? DimensionNumber,
    string? ItemBatchNumber,
    string? ProductConfigurationId,
    string? OrderedInventoryStatusId,
    DateTimeOffset? ConfirmedShippingDate,
    string? InventoryWarehouseId,
    string? ProductDescription)
    : ODataElement(Etag, DataAreaId), IODataElement
{
    /// <summary>
    /// Entities the name.
    /// </summary>
    /// <returns>System.String.</returns>
    public static string EntityName()
    {
        return "SalesInvoiceLines";
    }
}