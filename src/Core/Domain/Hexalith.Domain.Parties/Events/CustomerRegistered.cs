// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-21-2023
// ***********************************************************************
// <copyright file="CustomerRegistered.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.Events;

using System.Runtime.Serialization;

using Hexalith.Domain.ValueObjets;

/// <summary>
/// Class CustomerRegistered.
/// Implements the <see cref="Hexalith.Domain.Events.CustomerEvent" />.
/// </summary>
/// <seealso cref="Hexalith.Domain.Events.CustomerEvent" />
[DataContract]
public abstract class CustomerRegistered : CustomerEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerRegistered" /> class.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="name">The name.</param>
    /// <param name="contact">The contact.</param>
    /// <param name="invoiceAddress">The invoice address.</param>
    /// <param name="deliveryAddress">The delivery address.</param>
    /// <param name="externalIds">The external ids.</param>
    public CustomerRegistered(string id, string name, Contact contact, PostalAddress invoiceAddress, PostalAddress deliveryAddress, Dictionary<string, string> externalIds)
        : base(id)
    {
        Name = name;
        Contact = contact;
        InvoiceAddress = invoiceAddress;
        DeliveryAddress = deliveryAddress;
        ExternalIds = externalIds;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerRegistered" /> class.
    /// </summary>
    [Obsolete("This constructor is only for serialization purposes.", true)]
    public CustomerRegistered()
    {
        Name = string.Empty;
        Contact = new Contact();
        InvoiceAddress = new PostalAddress();
        DeliveryAddress = new PostalAddress();
    }

    /// <summary>
    /// Gets or sets the contact.
    /// </summary>
    /// <value>The contact.</value>
    [DataMember(Order = 11)]
    public Contact Contact { get; set; }

    /// <summary>
    /// Gets or sets the delivery address.
    /// </summary>
    /// <value>The delivery address.</value>
    [DataMember(Order = 13)]
    public PostalAddress DeliveryAddress { get; set; }

    /// <summary>
    /// Gets or sets the external ids.
    /// </summary>
    /// <value>The external ids.</value>
    [DataMember(Order = 14)]
    public Dictionary<string, string> ExternalIds { get; set; } = new();

    /// <summary>
    /// Gets or sets the invoice address.
    /// </summary>
    /// <value>The invoice address.</value>
    [DataMember(Order = 12)]
    public PostalAddress InvoiceAddress { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>The name.</value>
    [DataMember(Order = 10)]
    public string Name { get; set; } = string.Empty;
}