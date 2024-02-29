// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-21-2023
// ***********************************************************************
// <copyright file="Contact.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.ValueObjets;

using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

/// <summary>
/// Class Contact.
/// </summary>
[DataContract]
[Serializable]
public class Contact
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Contact" /> class.
    /// </summary>
    /// <param name="person">The person.</param>
    /// <param name="postalAddress">The postal address.</param>
    /// <param name="email">The email.</param>
    /// <param name="phone">The phone.</param>
    /// <param name="mobile">The mobile.</param>
    [JsonConstructor]
    public Contact(
        Person? person,
        PostalAddress? postalAddress,
        string? email,
        string? phone,
        string? mobile)
    {
        Person = (person == null) ? null : new Person(person);
        PostalAddress = (postalAddress == null) ? null : new PostalAddress(postalAddress);
        Email = email;
        Phone = phone;
        Mobile = mobile;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Contact"/> class.
    /// </summary>
    /// <param name="contact">The contact.</param>
    public Contact(Contact contact)
        : this(
              (contact?.Person == null) ? null : new Person(contact.Person),
              (contact?.PostalAddress == null) ? null : new PostalAddress(contact.PostalAddress),
              (contact ?? throw new ArgumentNullException(nameof(contact))).Email,
              contact.Phone,
              contact.Mobile)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Contact" /> class.
    /// </summary>
    public Contact()
    {
    }

    /// <summary>
    /// Gets or sets the email.
    /// </summary>
    /// <value>The email.</value>
    [DataMember(Order = 3)]
    [JsonPropertyOrder(3)]
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the mobile.
    /// </summary>
    /// <value>The mobile.</value>
    [DataMember(Order = 5)]
    [JsonPropertyOrder(5)]
    public string? Mobile { get; set; }

    /// <summary>
    /// Gets or sets the person.
    /// </summary>
    /// <value>The person.</value>
    [DataMember(Order = 1)]
    [JsonPropertyOrder(1)]
    public Person? Person { get; set; }

    /// <summary>
    /// Gets or sets the phone.
    /// </summary>
    /// <value>The phone.</value>
    [DataMember(Order = 4)]
    [JsonPropertyOrder(4)]
    public string? Phone { get; set; }

    /// <summary>
    /// Gets or sets the postal address.
    /// </summary>
    /// <value>The postal address.</value>
    [DataMember(Order = 2)]
    [JsonPropertyOrder(2)]
    public PostalAddress? PostalAddress { get; set; }

    /// <summary>
    /// Ares the same.
    /// </summary>
    /// <param name="a">a.</param>
    /// <param name="b">The b.</param>
    /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
    public static bool AreSame(Contact? a, Contact? b)
    {
        return a is null
            ? b is null
            : a == b || (
                Person.AreSame(a.Person, b?.Person) &&
                PostalAddress.AreSame(a.PostalAddress, b?.PostalAddress) &&
                a.Email == b?.Email &&
                a.Phone == b?.Phone &&
                a.Mobile == b?.Mobile);
    }
}