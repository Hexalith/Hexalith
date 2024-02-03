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

namespace Hexalith.UI.Parties.Components.Customer;

using System.ComponentModel.DataAnnotations;

using Hexalith.Application.Parties.Commands;
using Hexalith.Domain.ValueObjets;
using Hexalith.UI.PostalAddresses.Models;

/// <summary>
/// Class RegisterCustomerModel.
/// </summary>
internal class RegisterCustomerModel
{
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
    [Display(Description = "Contact person first name", Name = "First Name")]
    public string? FirstName { get; set; }

    /// <summary>
    /// Gets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    [Display(Description = "Customer identifier", Name = "Identifier")]
    public string Id => FirstName?[..Math.Min(FirstName.Length, 3)] + LastName?[..Math.Min(LastName.Length, 7)] ?? string.Empty;

    /// <summary>
    /// Gets or sets the last name.
    /// </summary>
    /// <value>The last name.</value>
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
    /// Gets or sets the address.
    /// </summary>
    /// <value>The address.</value>
    [Display(Description = "Customer postal address", Name = "Address")]
    public PostalAddressViewModel PostalAddress { get; set; } = new PostalAddressViewModel();

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
            PostalAddress.ToPostalAddress(),
            Email,
            PhoneNumber,
            MobilePhoneNumber),
        null,
        null,
        null,
        null,
        DateTimeOffset.Now);
}