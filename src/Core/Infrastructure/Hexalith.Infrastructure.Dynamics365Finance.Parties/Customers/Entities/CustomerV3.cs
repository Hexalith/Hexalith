// ***********************************************************************
// Assembly         : Christofle.Infrastructure.Dynamics365Finance.RestClient
// Author           : Jérôme Piquot
// Created          : 01-13-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-08-2023
// ***********************************************************************
// <copyright file="CustomerV3.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Entities;

using System.Runtime.Serialization;

using Hexalith.Infrastructure.Dynamics365Finance.Models;

/// <summary>
/// Class CustomerExternalCode.
/// Implements the <see cref="ODataElement" />
/// Implements the <see cref="IEquatable{ODataElement}" />
/// Implements the <see cref="IODataElement" />
/// Implements the <see cref="IEquatable{CustomerExternalCode}" />.
/// </summary>
/// <seealso cref="ODataElement" />
/// <seealso cref="IEquatable{ODataElement}" />
/// <seealso cref="IODataElement" />
/// <seealso cref="IEquatable{CustomerExternalCode}" />
[DataContract]
[Serializable]
public record CustomerV3
(
    string DataAreaId,
    string? CustomerAccount = null,
    string? Etag = null,
    string? CustomerGroupId = null,
    string? PartyType = null,
    string? OrganizationName = null,
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
    string? PrimaryContactPhoneIsMobile = null,
    string? PrimaryContactEmail = null,
    string? PersonLastName = null,
    string? PersonFirstName = null,
    string? PersonGender = null,
    string? PersonPersonalTitle = null)

: ODataElement(Etag, DataAreaId), IODataElement
{
    /// <summary>
    /// Entities the name.
    /// </summary>
    /// <returns>System.String.</returns>
    public static string EntityName() => "CustomersV3";
}