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
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
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
    [property: DataMember(Order = 3)] string SalesOrderNumber,
    [property: DataMember(Order = 4)] decimal LineNumber,
    [property: DataMember(Order = 5)] int CustomersLineNumber,
    [property: DataMember(Order = 6)] string ItemNumber,
    [property: DataMember(Order = 7)] string ProductStyleId,
    [property: DataMember(Order = 8)] string ProductColorId,
    [property: DataMember(Order = 9)] string ProductSizeId,
    [property: DataMember(Order = 10)] string ProductConfigurationId,
    [property: DataMember(Order = 11)] string ProductVersionId,
    [property: DataMember(Order = 12)] decimal SalesPrice,
    [property: DataMember(Order = 13)] decimal OrderedSalesQuantity,
    [property: DataMember(Order = 14)] string SalesUnitSymbol,
    [property: DataMember(Order = 15)] string GiftCardGiftMessage,
    [property: DataMember(Order = 16)] string InventoryLotId)
    : ODataElement(Etag, DataAreaId), IODataElement
{
    /// <summary>
    /// Entities the name.
    /// </summary>
    /// <returns>System.String.</returns>
    public static string EntityName() => "SalesOrderLines";
}