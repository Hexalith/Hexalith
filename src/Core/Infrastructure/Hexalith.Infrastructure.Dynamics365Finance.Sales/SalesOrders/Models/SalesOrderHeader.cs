// ***********************************************************************
// Assembly         : Christofle.Infrastructure.Dynamics365Finance.RestClient
// Author           : Jérôme Piquot
// Created          : 01-13-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-25-2023
// ***********************************************************************
// <copyright file="SalesOrderHeader.cs" company="Fiveforty SAS Paris France">
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
/// Class SalesOrderHeader.
/// Implements the <see cref="ODataElement" />
/// Implements the <see cref="IEquatable{ODataElement}" />
/// Implements the <see cref="IODataElement" />
/// Implements the <see cref="IEquatable{SalesOrderHeader}" />.
/// </summary>
/// <seealso cref="ODataElement" />
/// <seealso cref="IEquatable{ODataElement}" />
/// <seealso cref="IODataElement" />
/// <seealso cref="IEquatable{SalesOrderHeader}" />
[DataContract]
public record SalesOrderHeader(
    string Etag,
    string DataAreaId,
    [property: DataMember(Order = 3)] string SalesOrderNumber,
    [property: DataMember(Order = 4)] SalesStatus SalesOrderStatus,
    [property: DataMember(Order = 5)] string SalesOrderOriginCode,
    [property: DataMember(Order = 6)] string CustomerRequisitionNumber,
    [property: DataMember(Order = 7)] string CustomersOrderReference,
    [property: DataMember(Order = 8)] string OrderingCustomerAccountNumber,
    [property: DataMember(Order = 9)] string InvoiceCustomerAccountNumber,
    [property: DataMember(Order = 10)] DateTimeOffset OrderCreationDateTime,
    [property: DataMember(Order = 11)] string SalesUnitId,
    [property: DataMember(Order = 12)] string RevRecReallocationId,
    [property: DataMember(Order = 13)] string DeliveryAddressName,
    [property: DataMember(Order = 14)] string DeliveryAddressDescription,
    [property: DataMember(Order = 15)] string DeliveryAddressStreetNumber,
    [property: DataMember(Order = 16)] string DeliveryAddressStreet,
    [property: DataMember(Order = 17)] string DeliveryAddressPostBox,
    [property: DataMember(Order = 18)] string DeliveryAddressZipCode,
    [property: DataMember(Order = 19)] string DeliveryAddressCity,
    [property: DataMember(Order = 20)] string DeliveryAddressCountyId,
    [property: DataMember(Order = 21)] string DeliveryAddressStateId,
    [property: DataMember(Order = 22)] string DeliveryAddressCountryRegionISOCode,
    [property: DataMember(Order = 23)] string Email,
    [property: DataMember(Order = 24)] DateTimeOffset RequestedShippingDate,
    [property: DataMember(Order = 25)] DateTimeOffset RequestedReceiptDate,
    [property: DataMember(Order = 26)] string ShippingCarrierId,
    [property: DataMember(Order = 27)] string ShippingCarrierServiceId)
    : ODataElement(Etag, DataAreaId), IODataElement
{
    /// <summary>
    /// Entities the name.
    /// </summary>
    /// <returns>System.String.</returns>
    public static string EntityName() => "SalesOrderHeadersV2";
}