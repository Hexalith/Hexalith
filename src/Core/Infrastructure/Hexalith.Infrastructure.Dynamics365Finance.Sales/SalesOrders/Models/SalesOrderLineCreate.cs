// ***********************************************************************
// Assembly         : Christofle.Infrastructure.Dynamics365Finance.RestClient
// Author           : Jérôme Piquot
// Created          : 11-18-2022
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 12-17-2022
// ***********************************************************************
// <copyright file="SalesOrderLineCreate.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Dynamics365Finance.Sales.SalesOrders.Models;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

/// <summary>
/// Class SalesOrderLineCreate.
/// Implements the <see cref="IEquatable{SalesOrderLineCreate}" />.
/// </summary>
/// <seealso cref="IEquatable{SalesOrderLineCreate}" />
[DataContract]
public record SalesOrderLineCreate(
        [property: DataMember(Order = 1)][property: JsonPropertyName("dataAreaId")] string DataAreaId,
        [property: DataMember(Order = 2)] string SalesOrderNumber,
        [property: DataMember(Order = 3)] int? CustomersLineNumber,
        [property: DataMember(Order = 4)] string ItemNumber,
        [property: DataMember(Order = 5)] string? ProductStyleId,
        [property: DataMember(Order = 6)] string? ProductColorId,
        [property: DataMember(Order = 7)] string? ProductSizeId,
        [property: DataMember(Order = 8)] decimal SalesPrice,
        [property: DataMember(Order = 9)] decimal OrderedSalesQuantity,
        [property: DataMember(Order = 10)] string? SalesUnitSymbol,
        [property: DataMember(Order = 11)] string? GiftCardGiftMessage)
{
}