// ***********************************************************************
// Assembly         : Christofle.Infrastructure.Dynamics365Finance.RestClient
// Author           : Jérôme Piquot
// Created          : 01-13-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 12-14-2023
// ***********************************************************************
// <copyright file="CustomerV3Create.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Entities;

using System.Runtime.Serialization;
using System.Runtime.Serialization.DataContracts;
using System.Text.Json.Serialization;

/// <summary>
/// Class CustomerV3Create.
/// </summary>
[DataContract]
[Serializable]
public class CustomerV3Create
{
    /// <summary>
    /// Gets or sets the address books.
    /// </summary>
    /// <value>The address books.</value>
    [DataMember]
    public string? AddressBooks { get; set; }

    /// <summary>
    /// Gets or sets the address city.
    /// </summary>
    /// <value>The address city.</value>
    [DataMember]
    public string? AddressCity { get; set; }

    /// <summary>
    /// Gets or sets the address country region identifier.
    /// </summary>
    /// <value>The address country region identifier.</value>
    [DataMember]
    public string? AddressCountryRegionId { get; set; }

    /// <summary>
    /// Gets or sets the primary address country region iso code.
    /// </summary>
    /// <value>The primary address country region iso code.</value>
    [DataMember]
    public string? AddressCountryRegionISOCode { get; set; }

    /// <summary>
    /// Gets or sets the primary address county.
    /// </summary>
    /// <value>The primary address county.</value>
    [DataMember]
    public string? AddressCounty { get; set; }

    /// <summary>
    /// Gets or sets the address description.
    /// </summary>
    /// <value>The address description.</value>
    [DataMember]
    public string? AddressDescription { get; set; }

    /// <summary>
    /// Gets or sets the primary address street.
    /// </summary>
    /// <value>The primary address street.</value>
    [DataMember]
    public string? AddressStreet { get; set; }

    /// <summary>
    /// Gets or sets the address street number.
    /// </summary>
    /// <value>The address street number.</value>
    [DataMember]
    public string? AddressStreetNumber { get; set; }

    /// <summary>
    /// Gets or sets the address zip code.
    /// </summary>
    /// <value>The address zip code.</value>
    [DataMember]
    public string? AddressZipCode { get; set; }

    /// <summary>
    /// Gets or sets the commission sales group identifier.
    /// </summary>
    /// <value>The commission sales group identifier.</value>
    [DataMember]
    public string? CommissionSalesGroupId { get; set; }

    /// <summary>
    /// Gets or sets the customer group identifier.
    /// </summary>
    /// <value>The customer group identifier.</value>
    [DataMember]
    public string? CustomerGroupId { get; set; }

    /// <summary>
    /// Gets or sets the data area identifier.
    /// </summary>
    /// <value>The data area identifier.</value>
    [DataMember]
    [JsonPropertyName("dataAreaId")]
    public string? DataAreaId { get; set; }

    /// <summary>
    /// Gets or sets the default dimension display value.
    /// </summary>
    /// <value>The default dimension display value.</value>
    [DataMember]
    public string? DefaultDimensionDisplayValue { get; set; }

    /// <summary>
    /// Gets or sets the name alias.
    /// </summary>
    /// <value>The name alias.</value>
    [DataMember]
    public string? NameAlias { get; set; }

    /// <summary>
    /// Gets or sets the name of the organization.
    /// </summary>
    /// <value>The name of the organization.</value>
    [DataMember]
    public string? OrganizationName { get; set; }

    /// <summary>
    /// Gets or sets the type of the party.
    /// </summary>
    /// <value>The type of the party.</value>
    [DataMember]
    public string? PartyType { get; set; }

    /// <summary>
    /// Gets or sets the person birth day.
    /// </summary>
    /// <value>The person birth day.</value>
    [DataMember]
    public int? PersonBirthDay { get; set; }

    /// <summary>
    /// Gets or sets the person birth month.
    /// </summary>
    /// <value>The person birth month.</value>
    [DataMember]
    public int? PersonBirthMonth { get; set; }

    /// <summary>
    /// Gets or sets the person brith year.
    /// </summary>
    /// <value>The person brith year.</value>
    [DataMember]
    public int? PersonBirthYear { get; set; }

    /// <summary>
    /// Gets or sets the first name of the person.
    /// </summary>
    /// <value>The first name of the person.</value>
    [DataMember]
    public string? PersonFirstName { get; set; }

    /// <summary>
    /// Gets or sets the person gender.
    /// </summary>
    /// <value>The person gender.</value>
    [DataMember]
    public string? PersonGender { get; set; }

    /// <summary>
    /// Gets or sets the last name of the person.
    /// </summary>
    /// <value>The last name of the person.</value>
    [DataMember]
    public string? PersonLastName { get; set; }

    /// <summary>
    /// Gets or sets the person personal title.
    /// </summary>
    /// <value>The person personal title.</value>
    [DataMember]
    public string? PersonPersonalTitle { get; set; }

    /// <summary>
    /// Gets or sets the primary contact email.
    /// </summary>
    /// <value>The primary contact email.</value>
    [DataMember]
    public string? PrimaryContactEmail { get; set; }

    /// <summary>
    /// Gets or sets the primary contact phone.
    /// </summary>
    /// <value>The primary contact phone.</value>
    [DataMember]
    public string? PrimaryContactPhone { get; set; }

    /// <summary>
    /// Gets or sets the primary contact phone extension.
    /// </summary>
    /// <value>The primary contact phone extension.</value>
    [DataMember]
    public string? PrimaryContactPhoneExtension { get; set; }

    /// <summary>
    /// Gets or sets the primary contact phone is mobile.
    /// </summary>
    /// <value>The primary contact phone is mobile.</value>
    [DataMember]
    public string? PrimaryContactPhoneIsMobile { get; set; }

    /// <summary>
    /// Gets or sets the sales currency code.
    /// </summary>
    /// <value>The sales currency code.</value>
    [DataMember]
    public string? SalesCurrencyCode { get; set; }

    /// <summary>
    /// Gets or sets the sales tax group.
    /// </summary>
    /// <value>The sales tax group.</value>
    [DataMember]
    public string? SalesTaxGroup { get; set; }

    /// <summary>
    /// Gets or sets the warehouse identifier.
    /// </summary>
    /// <value>The warehouse identifier.</value>
    [DataMember]
    public string? WarehouseId { get; set; }
}