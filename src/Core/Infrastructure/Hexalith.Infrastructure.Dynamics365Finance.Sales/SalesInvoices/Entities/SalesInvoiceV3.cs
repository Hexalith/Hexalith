// ***********************************************************************
// Assembly         : Christofle.Infrastructure.Dynamics365Finance.RestClient
// Author           : Jérôme Piquot
// Created          : 01-13-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-08-2023
// ***********************************************************************
// <copyright file="SalesInvoiceV3.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Dynamics365Finance.Sales.SalesInvoices.Entities;

using System.Runtime.Serialization;

using Hexalith.Infrastructure.Dynamics365Finance.Models;

/// <summary>
/// Class SalesInvoiceExternalCode.
/// Implements the <see cref="ODataElement" />
/// Implements the <see cref="IEquatable{ODataElement}" />
/// Implements the <see cref="IODataElement" />
/// Implements the <see cref="IEquatable{SalesInvoiceExternalCode}" />.
/// </summary>
/// <seealso cref="ODataElement" />
/// <seealso cref="IEquatable{ODataElement}" />
/// <seealso cref="IODataElement" />
/// <seealso cref="IEquatable{SalesInvoiceExternalCode}" />
[DataContract]
public record SalesInvoiceV3
(
    string DataAreaId,
    [property: DataMember(Order = 3)] string? SalesInvoiceAccount = null,
    string? Etag = null,
    [property: DataMember(Order = 4)] string? SalesInvoiceGroupId = null,
    [property: DataMember(Order = 5)] string? PartyType = null,
    [property: DataMember(Order = 6)] string? OrganizationName = null,
    [property: DataMember(Order = 7)] string? NameAlias = null,
    [property: DataMember(Order = 8)] string? SalesCurrencyCode = null,
    [property: DataMember(Order = 9)] string? WarehouseId = null,
    [property: DataMember(Order = 10)] string? CommissionSalesGroupId = null,
    [property: DataMember(Order = 11)] string? AddressDescription = null,
    [property: DataMember(Order = 12)] string? AddressStreetNumber = null,
    [property: DataMember(Order = 13)] string? AddressStreet = null,
    [property: DataMember(Order = 14)] string? AddressState = null,
    [property: DataMember(Order = 15)] string? AddressCounty = null,
    [property: DataMember(Order = 16)] string? AddressCity = null,
    [property: DataMember(Order = 17)] string? AddressZipCode = null,
    [property: DataMember(Order = 18)] string? AddressCountryRegionId = null,
    [property: DataMember(Order = 19)] string? AddressCountryRegionISOCode = null,
    [property: DataMember(Order = 20)] string? PrimaryContactPhoneExtension = null,
    [property: DataMember(Order = 21)] string? PrimaryContactPhone = null,
    [property: DataMember(Order = 22)] string? PrimaryContactPhoneIsMobile = null,
    [property: DataMember(Order = 23)] string? PrimaryContactEmail = null,
    [property: DataMember(Order = 24)] string? PersonLastName = null,
    [property: DataMember(Order = 25)] string? PersonFirstName = null,
    [property: DataMember(Order = 26)] string? PersonGender = null,
    [property: DataMember(Order = 27)] string? SalesTaxGroup = null,
    [property: DataMember(Order = 28)] string? DefaultDimensionDisplayValue = null,
    [property: DataMember(Order = 29)] string? AddressBooks = null)

: ODataElement(Etag, DataAreaId), IODataElement
{
    /// <summary>
    /// Entities the name.
    /// </summary>
    /// <returns>System.String.</returns>
    public static string EntityName() => "SalesInvoicesV3";
}