// ***********************************************************************
// Assembly         : HexalithApplication
// Author           : Jérôme Piquot
// Created          : 01-16-2024
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-16-2024
// ***********************************************************************
// <copyright file="RegisterCustomerModel.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace HexalithApplication.Components.Customer;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using Hexalith.Application.Parties.Commands;
using Hexalith.Domain.ValueObjets;

/// <summary>
/// Class RegisterCustomerModel.
/// </summary>
internal class RegisterCustomerModel
{
    /// <summary>
    /// Gets or sets the address.
    /// </summary>
    /// <value>The address.</value>
    [Display(Description = "Address lines", Name = nameof(Address))]
    public string? Address { get; set; }

    /// <summary>
    /// Gets or sets the city.
    /// </summary>
    /// <value>The city.</value>
    [DisplayName]
    public string? City { get; set; }

    /// <summary>
    /// Gets or sets the country identifier.
    /// </summary>
    /// <value>The country identifier.</value>
    [Display(Description = "Country ISO 3166-A3 three letters code (USA,FRA,GIB,DEU,...)", Name = "Country")]
    [System.ComponentModel.DataAnnotations.Length(3, 3)]
    public string? CountryId { get; set; }

    /// <summary>
    /// Gets or sets the email address.
    /// </summary>
    /// <value>The email address.</value>
    [EmailAddress]
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the first name.
    /// </summary>
    /// <value>The first name.</value>
    [Required]
    [Display(Description = "Contact person first name", Name = "First Name")]
    public string? FirstName { get; set; }

    /// <summary>
    /// Gets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    [Display(Description = "Customer identifier", Name = "Identifier")]
    public string Id => (FirstName?[..Math.Min(FirstName.Length, 3)] + LastName?[..Math.Min(LastName.Length, 7)]) ?? string.Empty;

    /// <summary>
    /// Gets or sets the last name.
    /// </summary>
    /// <value>The last name.</value>
    [Required]
    [Display(Description = "Contact person last name", Name = "Last Name")]
    public string? LastName { get; set; }

    /// <summary>
    /// Gets or sets the mobile phone number.
    /// </summary>
    /// <value>The mobile phone number.</value>
    [Display(Description = "Contact mobile phone number", Name = "Mobile")]
    public string? MobilePhoneNumber { get; set; }

    /// <summary>
    /// Gets the name.
    /// </summary>
    /// <value>The name.</value>
    [Display(Description = "Customer name", Name = "Name")]
    public string Name => FirstName + " " + LastName;

    /// <summary>
    /// Gets or sets the phone number.
    /// </summary>
    /// <value>The phone number.</value>
    [Display(Description = "Contact phone number", Name = "Phone")]
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets the zip code.
    /// </summary>
    /// <value>The zip code.</value>
    [Display(Description = "Postal address zip code", Name = "ZipCode")]
    public string? ZipCode { get; set; }

    /// <summary>
    /// Converts to command.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="originId">The origin identifier.</param>
    /// <returns>RegisterCustomer.</returns>
    public RegisterCustomer ToCommand(string partitionId, string companyId, string originId) => new(
        partitionId,
        companyId,
        originId,
        Id,
        Name,
        PartyType.Person,
        new Contact(
            new Person(Name, FirstName, LastName, null, null, null),
            new PostalAddress(
                Name,
                Name,
                null,
                Address,
                null,
                ZipCode,
                City,
                null,
                null,
                null,
                CountryId,
                null,
                null),
            Email,
            PhoneNumber,
            MobilePhoneNumber),
        null,
        null,
        null,
        null,
        DateTimeOffset.Now);
}