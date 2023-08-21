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
        Person = person;
        PostalAddress = postalAddress;
        Email = email;
        Phone = phone;
        Mobile = mobile;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Contact" /> class.
    /// </summary>
    [Obsolete("This constructor is only for serialization purposes.", true)]
    public Contact()
    {
    }

    /// <summary>
    /// Gets the email.
    /// </summary>
    /// <value>The email.</value>
    [DataMember(Order = 3)]
    [JsonPropertyOrder(3)]
    public string? Email { get; private set; }

    /// <summary>
    /// Gets the mobile.
    /// </summary>
    /// <value>The mobile.</value>
    [DataMember(Order = 5)]
    [JsonPropertyOrder(5)]
    public string? Mobile { get; private set; }

    /// <summary>
    /// Gets the person.
    /// </summary>
    /// <value>The person.</value>
    [DataMember(Order = 1)]
    [JsonPropertyOrder(1)]
    public Person? Person { get; private set; }

    /// <summary>
    /// Gets the phone.
    /// </summary>
    /// <value>The phone.</value>
    [DataMember(Order = 4)]
    [JsonPropertyOrder(4)]
    public string? Phone { get; private set; }

    /// <summary>
    /// Gets the postal address.
    /// </summary>
    /// <value>The postal address.</value>
    [DataMember(Order = 2)]
    [JsonPropertyOrder(2)]
    public PostalAddress? PostalAddress { get; private set; }
}