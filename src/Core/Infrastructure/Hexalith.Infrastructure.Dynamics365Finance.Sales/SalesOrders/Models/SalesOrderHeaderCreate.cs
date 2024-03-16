// ***********************************************************************
// Assembly         : Christofle.Infrastructure.Dynamics365Finance.RestClient
// Author           : Jérôme Piquot
// Created          : 01-13-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-25-2023
// ***********************************************************************
// <copyright file="SalesOrderHeaderCreate.cs" company="Fiveforty SAS Paris France">
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
/// Class SalesOrderHeaderCreate.
/// Implements the <see cref="IEquatable{SalesOrderHeaderCreate}" />.
/// </summary>
/// <seealso cref="IEquatable{SalesOrderHeaderCreate}" />
[DataContract]
public record SalesOrderHeaderCreate(
        [property: DataMember(Order = 1)][property: JsonPropertyName("dataAreaId")] string DataAreaId,
        [property: DataMember(Order = 2)] string? SalesOrderOriginCode,
        [property: DataMember(Order = 3)] string? CustomerRequisitionNumber,
        [property: DataMember(Order = 4)] string? CustomersOrderReference,
        [property: DataMember(Order = 5)] string? OrderingCustomerAccountNumber,
        [property: DataMember(Order = 6)] string? InvoiceCustomerAccountNumber,
        [property: DataMember(Order = 7)] string? SalesUnitId,
        [property: DataMember(Order = 8)] string? RevRecReallocationId,
        [property: DataMember(Order = 9)] string? DeliveryAddressName,
        [property: DataMember(Order = 10)] string? DeliveryAddressDescription,
        [property: DataMember(Order = 11)] string? DeliveryAddressStreetNumber,
        [property: DataMember(Order = 12)] string? DeliveryAddressStreet,
        [property: DataMember(Order = 13)] string? DeliveryAddressPostBox,
        [property: DataMember(Order = 14)] string? DeliveryAddressZipCode,
        [property: DataMember(Order = 15)] string? DeliveryAddressCity,
        [property: DataMember(Order = 16)] string? DeliveryAddressCountyId,
        [property: DataMember(Order = 17)] string? DeliveryAddressStateId,
        [property: DataMember(Order = 18)][property: JsonPropertyName("DeliveryAddressCountryRegionISOCode")] string? DeliveryAddressCountryRegionISOCode,
        [property: DataMember(Order = 19)] string? DeliveryAddressCountryRegionId,
        [property: DataMember(Order = 20)] string? Email,
        [property: DataMember(Order = 21)] DateTimeOffset? RequestedShippingDate,
        [property: DataMember(Order = 22)] DateTimeOffset? RequestedReceiptDate)
{
}