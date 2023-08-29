// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-29-2023
// ***********************************************************************
// <copyright file="ChangeCustomerInformation.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Parties.Commands;

using System.Runtime.Serialization;

using Hexalith.Domain.ValueObjets;

/// <summary>
/// Class ChangeCustomerInformation.
/// Implements the <see cref="CustomerCommand" />.
/// </summary>
/// <seealso cref="CustomerCommand" />
[DataContract]
public class ChangeCustomerInformation : CustomerCommand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChangeCustomerInformation" /> class.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="name">The name.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="contact">The contact.</param>
    /// <param name="invoiceAddress">The invoice address.</param>
    /// <param name="deliveryAddress">The delivery address.</param>
    /// <param name="date">The external ids.</param>
    public ChangeCustomerInformation(
        string id,
        string name,
        string companyId,
        Contact contact,
        PostalAddress invoiceAddress,
        PostalAddress deliveryAddress,
        DateTimeOffset date)
        : base(id)
    {
        Name = name;
        CompanyId = companyId;
        Contact = contact;
        InvoiceAddress = invoiceAddress;
        DeliveryAddress = deliveryAddress;
        Date = date;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ChangeCustomerInformation" /> class.
    /// </summary>
    [Obsolete("This constructor is only for serialization purposes.", true)]
    public ChangeCustomerInformation()
    {
        Name = string.Empty;
        Contact = new Contact();
        InvoiceAddress = new PostalAddress();
        DeliveryAddress = new PostalAddress();
        Date = DateTimeOffset.MinValue;
    }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>The name.</value>
    [DataMember(Order = 11)]
    public string CompanyId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the contact.
    /// </summary>
    /// <value>The contact.</value>
    [DataMember(Order = 12)]
    public Contact Contact { get; set; }

    /// <summary>
    /// Gets or sets the external ids.
    /// </summary>
    /// <value>The external ids.</value>
    [DataMember(Order = 15)]
    public DateTimeOffset Date { get; set; }

    /// <summary>
    /// Gets or sets the delivery address.
    /// </summary>
    /// <value>The delivery address.</value>
    [DataMember(Order = 14)]
    public PostalAddress DeliveryAddress { get; set; }

    /// <summary>
    /// Gets or sets the invoice address.
    /// </summary>
    /// <value>The invoice address.</value>
    [DataMember(Order = 13)]
    public PostalAddress InvoiceAddress { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>The name.</value>
    [DataMember(Order = 10)]
    public string Name { get; set; } = string.Empty;
}