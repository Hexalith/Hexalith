// ***********************************************************************
// Assembly         : Christofle.Infrastructure.Dynamics365Finance.RestClient
// Author           : Jérôme Piquot
// Created          : 03-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 03-04-2023
// ***********************************************************************
// <copyright file="SalesOrderHeaderEdiInformation.cs" company="Fiveforty">
//     Copyright (c) Fiveforty S.A.S.. All rights reserved.
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
    string SalesOrderNumber,
    string? FFYReservationNum,
    string? FFYStore,
    string? FFYDepartment,
    string? FFYServiceCode,
    string? FFYShippingServiceLevel,
    string? FFYCustomerOrderNumber,
    string? FFYBillToCode,
    string? FFYBillToName,
    string? FFYBillToAddress1,
    string? FFYBillToAddress2,
    string? FFYBillToPostal,
    string? FFYBillToCountry,
    string? FFYBillToCity,
    string? FFYBillToState,
    DateTimeOffset? FFYOrderDate)
    : ODataElement(Etag, DataAreaId), IODataElement
{
    /// <summary>
    /// Entities the name.
    /// </summary>
    /// <returns>System.String.</returns>
    public static string EntityName()
    {
        return "FFYSalesOrderHeaderEDIinfos";
    }
}