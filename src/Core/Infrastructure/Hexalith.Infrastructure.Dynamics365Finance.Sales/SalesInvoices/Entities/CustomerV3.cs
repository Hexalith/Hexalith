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
[Serializable]
public record SalesInvoiceV3
(
    string DataAreaId,
    string? SalesInvoiceAccount = null,
    string? Etag = null,
    string? SalesInvoiceGroupId = null,
    string? PartyType = null,
    string? OrganizationName = null,
    string? NameAlias = null,
    string? SalesCurrencyCode = null,
    string? WarehouseId = null,
    string? CommissionSalesGroupId = null,
    string? AddressDescription = null,
    string? AddressStreetNumber = null,
    string? AddressStreet = null,
    string? AddressState = null,
    string? AddressCounty = null,
    string? AddressCity = null,
    string? AddressZipCode = null,
    string? AddressCountryRegionId = null,
    string? AddressCountryRegionISOCode = null,
    string? PrimaryContactPhoneExtension = null,
    string? PrimaryContactPhone = null,
    string? PrimaryContactPhoneIsMobile = null,
    string? PrimaryContactEmail = null,
    string? PersonLastName = null,
    string? PersonFirstName = null,
    string? PersonGender = null,
    string? SalesTaxGroup = null,
    string? DefaultDimensionDisplayValue = null,
    string? AddressBooks = null)

: ODataElement(Etag, DataAreaId), IODataElement
{
    /// <summary>
    /// Entities the name.
    /// </summary>
    /// <returns>System.String.</returns>
    public static string EntityName() => "SalesInvoicesV3";
}