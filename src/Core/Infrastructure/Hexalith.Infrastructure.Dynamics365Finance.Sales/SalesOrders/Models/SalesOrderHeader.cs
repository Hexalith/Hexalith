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
    string SalesOrderNumber,
    SalesStatus SalesOrderStatus,
    string SalesOrderOriginCode,
    string CustomerRequisitionNumber,
    string CustomersOrderReference,
    string OrderingCustomerAccountNumber,
    string InvoiceCustomerAccountNumber,
    DateTimeOffset OrderCreationDateTime,
    string SalesUnitId,
    string RevRecReallocationId,
    string DeliveryAddressName,
    string DeliveryAddressDescription,
    string DeliveryAddressStreetNumber,
    string DeliveryAddressStreet,
    string DeliveryAddressPostBox,
    string DeliveryAddressZipCode,
    string DeliveryAddressCity,
    string DeliveryAddressCountyId,
    string DeliveryAddressStateId,
    string DeliveryAddressCountryRegionISOCode,
    string Email,
    DateTimeOffset RequestedShippingDate,
    DateTimeOffset RequestedReceiptDate,
    string ShippingCarrierId,
    string ShippingCarrierServiceId)
    : ODataElement(Etag, DataAreaId), IODataElement
{
    /// <summary>
    /// Entities the name.
    /// </summary>
    /// <returns>System.String.</returns>
    public static string EntityName()
    {
        return "SalesOrderHeadersV2";
    }
}