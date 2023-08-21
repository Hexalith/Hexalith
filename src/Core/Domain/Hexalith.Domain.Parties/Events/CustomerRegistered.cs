// <copyright file="CustomerEvent.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Events;

using System.Runtime.Serialization;

using Hexalith.Domain.ValueObjets;

[DataContract]
public abstract class CustomerEvent : BaseEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerEvent"/> class.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="contact"></param>
    /// <param name="invoiceAddress"></param>
    /// <param name="deliveryAddress"></param>
    /// <param name="externalIds"></param>
    public CustomerEvent(string id, string name, Contact contact, PostalAddress invoiceAddress, PostalAddress deliveryAddress, Dictionary<string, string> externalIds)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerEvent"/> class.
    /// </summary>
    [Obsolete("This constructor is only for serialization purposes.", true)]
    public CustomerEvent()
    {
    }

    [DataMember(Order = 3)]
    public Contact Contact { get; set; }

    [DataMember(Order = 5)]
    public PostalAddress DeliveryAddress { get; set; }

    [DataMember(Order = 6)]
    public Dictionary<string, string> ExternalIds { get; set; } = new();

    [DataMember(Order = 1)]
    public string Id { get; set; } = string.Empty;

    [DataMember(Order = 4)]
    public PostalAddress InvoiceAddress { get; set; }

    [DataMember(Order = 2)]
    public string Name { get; set; } = string.Empty;
}