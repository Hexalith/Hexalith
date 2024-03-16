// ***********************************************************************
// Assembly         : Christofle.Infrastructure.Dynamics365Finance.RestClient
// Author           : Jérôme Piquot
// Created          : 03-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 03-04-2023
// ***********************************************************************
// <copyright file="SalesOrderHeaderEdiInformation.cs" company="Fiveforty SAS Paris France">
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
/// Class SalesOrderHeaderAdditional.
/// Implements the <see cref="ODataElement" />
/// Implements the <see cref="IEquatable{ODataElement}" />
/// Implements the <see cref="IODataElement" />
/// Implements the <see cref="IEquatable{SalesOrderHeaderAdditional}" />.
/// </summary>
/// <seealso cref="ODataElement" />
/// <seealso cref="IEquatable{ODataElement}" />
/// <seealso cref="IODataElement" />
/// <seealso cref="IEquatable{SalesOrderHeaderAdditional}" />
[DataContract]
public record SalesOrderHeaderEdiInformation(
    string Etag,
    string DataAreaId,
    [property: DataMember(Order = 3)] string SalesOrderNumber,
    [property: DataMember(Order = 4)] string? FFYReservationNum,
    [property: DataMember(Order = 5)] string? FFYStore,
    [property: DataMember(Order = 6)] string? FFYDepartment,
    [property: DataMember(Order = 7)] string? FFYServiceCode,
    [property: DataMember(Order = 8)] string? FFYShippingServiceLevel,
    [property: DataMember(Order = 9)] string? FFYCustomerOrderNumber,
    [property: DataMember(Order = 10)] string? FFYBillToCode,
    [property: DataMember(Order = 11)] string? FFYBillToName,
    [property: DataMember(Order = 12)] string? FFYBillToAddress1,
    [property: DataMember(Order = 13)] string? FFYBillToAddress2,
    [property: DataMember(Order = 14)] string? FFYBillToPostal,
    [property: DataMember(Order = 15)] string? FFYBillToCountry,
    [property: DataMember(Order = 16)] string? FFYBillToCity,
    [property: DataMember(Order = 17)] string? FFYBillToState,
    [property: DataMember(Order = 18)] DateTimeOffset? FFYOrderDate)
    : ODataElement(Etag, DataAreaId), IODataElement
{
    /// <summary>
    /// Entities the name.
    /// </summary>
    /// <returns>System.String.</returns>
    public static string EntityName() => "FFYSalesOrderHeaderEDIinfos";
}