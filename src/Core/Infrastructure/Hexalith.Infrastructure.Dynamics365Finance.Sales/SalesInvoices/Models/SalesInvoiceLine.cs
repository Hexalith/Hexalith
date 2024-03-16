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
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
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
    [property: DataMember(Order = 3)] string InvoiceNumber,
    [property: DataMember(Order = 4)] int LineCreationSequenceNumber,
    [property: DataMember(Order = 5)] DateTimeOffset InvoiceDate,
    [property: DataMember(Order = 6)] decimal InvoicedQuantity,
    [property: DataMember(Order = 7)] string ProductName,
    [property: DataMember(Order = 8)] decimal SalesPrice,
    [property: DataMember(Order = 9)] decimal LineTotalTaxAmount,
    [property: DataMember(Order = 10)] decimal LineAmount,
    [property: DataMember(Order = 11)] string CurrencyCode,
    [property: DataMember(Order = 12)] string SalesUnitSymbol,
    [property: DataMember(Order = 13)] string ProductNumber,
    [property: DataMember(Order = 14)] string? ProductSizeId,
    [property: DataMember(Order = 15)] string? ProductColorId,
    [property: DataMember(Order = 16)] string? ProductVersionId,
    [property: DataMember(Order = 17)] string? ProductStyleId,
    [property: DataMember(Order = 18)] decimal? LineTotalChargeAmount,
    [property: DataMember(Order = 19)] decimal? LineTotalDiscountAmount,
    [property: DataMember(Order = 20)] string? InventorySiteId,
    [property: DataMember(Order = 21)] string? DimensionNumber,
    [property: DataMember(Order = 22)] string? ItemBatchNumber,
    [property: DataMember(Order = 23)] string? ProductConfigurationId,
    [property: DataMember(Order = 24)] string? OrderedInventoryStatusId,
    [property: DataMember(Order = 25)] DateTimeOffset? ConfirmedShippingDate,
    [property: DataMember(Order = 26)] string? InventoryWarehouseId,
    [property: DataMember(Order = 27)] string? ProductDescription)
    : ODataElement(Etag, DataAreaId), IODataElement
{
    /// <summary>
    /// Entities the name.
    /// </summary>
    /// <returns>System.String.</returns>
    public static string EntityName() => "SalesInvoiceLines";
}