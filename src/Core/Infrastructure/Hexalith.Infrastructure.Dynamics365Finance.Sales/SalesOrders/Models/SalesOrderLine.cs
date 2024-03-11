// ***********************************************************************
// Assembly         : Christofle.Infrastructure.Dynamics365Finance.RestClient
// Author           : Jérôme Piquot
// Created          : 01-13-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-07-2023
// ***********************************************************************
// <copyright file="SalesOrderLine.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Dynamics365Finance.Sales.SalesOrders.Models;

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
public record SalesOrderLine(
    string Etag,
    string DataAreaId,
    string SalesOrderNumber,
    decimal LineNumber,
    int CustomersLineNumber,
    string ItemNumber,
    string ProductStyleId,
    string ProductColorId,
    string ProductSizeId,
    string ProductConfigurationId,
    string ProductVersionId,
    decimal SalesPrice,
    decimal OrderedSalesQuantity,
    string SalesUnitSymbol,
    string GiftCardGiftMessage,
    string InventoryLotId)
    : ODataElement(Etag, DataAreaId), IODataElement
{
    /// <summary>
    /// Entities the name.
    /// </summary>
    /// <returns>System.String.</returns>
    public static string EntityName()
    {
        return "SalesOrderLines";
    }
}