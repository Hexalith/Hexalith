// ***********************************************************************
// Assembly         : Christofle.Infrastructure.Dynamics365Finance.RestClient
// Author           : Jérôme Piquot
// Created          : 03-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 03-15-2023
// ***********************************************************************
// <copyright file="SalesOrderHeaderEdiInformationCreate.cs" company="Fiveforty SAS Paris France">
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
/// Class SalesOrderHeaderEdiInformationCreate.
/// Implements the <see cref="IEquatable{SalesOrderHeaderEdiInformationCreate}" />.
/// </summary>
/// <seealso cref="IEquatable{SalesOrderHeaderEdiInformationCreate}" />
[DataContract]
public record SalesOrderHeaderEdiInformationCreate(
        [property: DataMember(Order = 1)][property: JsonPropertyName("dataAreaId")] string DataAreaId,
        [property: DataMember(Order = 2)] string SalesOrderNumber,
        [property: DataMember(Order = 3)][property: JsonPropertyName("FFYReservationNum")] string? ReservationNumber,
        [property: DataMember(Order = 4)][property: JsonPropertyName("FFYStore")] string? StoreCode,
        [property: DataMember(Order = 5)][property: JsonPropertyName("FFYDepartment")] string? StoreDepartmentCode,
        [property: DataMember(Order = 6)][property: JsonPropertyName("FFYServiceCode")] string? ServiceCode,
        [property: DataMember(Order = 7)][property: JsonPropertyName("FFYShippingServiceLevel")] string? ShippingServiceLevel,
        [property: DataMember(Order = 8)][property: JsonPropertyName("FFYCustomerOrderNumber")] string? FinalCustomerOrderNumber,
        [property: DataMember(Order = 9)][property: JsonPropertyName("FFYBillToCode")] string? BillToCode,
        [property: DataMember(Order = 10)][property: JsonPropertyName("FFYBillToName")] string? BillToName,
        [property: DataMember(Order = 11)][property: JsonPropertyName("FFYBillToAddress")] string? BillToAddress1,
        [property: DataMember(Order = 12)][property: JsonPropertyName("FFYBillToAddress2")] string? BillToAddress2,
        [property: DataMember(Order = 13)][property: JsonPropertyName("FFYBillToPostal")] string? BillToPostal,
        [property: DataMember(Order = 14)][property: JsonPropertyName("FFYBillToCountry")] string? BillToCountry,
        [property: DataMember(Order = 15)][property: JsonPropertyName("FFYBillToCity")] string? BillToCity,
        [property: DataMember(Order = 16)][property: JsonPropertyName("FFYBillToState")] string? BillToState,
        [property: DataMember(Order = 17)][property: JsonPropertyName("FFYOrderDate")] DateTimeOffset? ShipOpenDate)
{
}