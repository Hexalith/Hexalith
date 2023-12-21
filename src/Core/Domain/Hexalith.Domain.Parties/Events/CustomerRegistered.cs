// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-30-2023
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
using System.Text.Json.Serialization;

using Hexalith.Domain.ValueObjets;
using Hexalith.Extensions;

/// <summary>
/// Class CustomerRegistered.
/// Implements the <see cref="Hexalith.Domain.Events.CustomerEvent" />.
/// </summary>
/// <seealso cref="Hexalith.Domain.Events.CustomerEvent" />
[DataContract]
[Serializable]
public class CustomerRegistered : CustomerEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerRegistered"/> class.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="originId">The origin identifier.</param>
    /// <param name="id">The identifier.</param>
    /// <param name="name">The name.</param>
    /// <param name="partyType">Type of the party.</param>
    /// <param name="contact">The contact.</param>
    /// <param name="warehouseId">The warehouse identifier.</param>
    /// <param name="commissionSalesGroupId">The commission sales group identifier.</param>
    /// <param name="groupId">The group identifier.</param>
    /// <param name="salesCurrencyId">The sales currency identifier.</param>
    /// <param name="date">The date.</param>
    [JsonConstructor]
    public CustomerRegistered(
        string partitionId,
        string companyId,
        string originId,
        string id,
        string name,
        PartyType partyType,
        Contact contact,
        string? warehouseId,
        string? commissionSalesGroupId,
        string? groupId,
        string? salesCurrencyId,
        DateTimeOffset date)
        : base(partitionId, companyId, originId, id)
    {
        Name = name;
        PartyType = partyType;
        Contact = contact;
        WarehouseId = warehouseId;
        CommissionSalesGroupId = commissionSalesGroupId;
        GroupId = groupId;
        SalesCurrencyId = salesCurrencyId;
        Date = date;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerRegistered" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    public CustomerRegistered()
    {
        Name = string.Empty;
        Contact = new Contact();
        Date = DateTimeOffset.MinValue;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerRegistered"/> class.
    /// </summary>
    /// <param name="customerRegistered">The customer registered.</param>
    public CustomerRegistered(CustomerRegistered customerRegistered)
        : this(
             (customerRegistered ?? throw new ArgumentNullException(nameof(customerRegistered))).PartitionId,
             customerRegistered.CompanyId,
             customerRegistered.OriginId,
             customerRegistered.Id,
             customerRegistered.Name,
             customerRegistered.PartyType,
             new Contact(customerRegistered.Contact),
             customerRegistered.WarehouseId,
             customerRegistered.CommissionSalesGroupId,
             customerRegistered.GroupId,
             customerRegistered.SalesCurrencyId,
             customerRegistered.Date)
    {
    }

    /// <summary>
    /// Gets or sets the commission sales group identifier.
    /// </summary>
    /// <value>The commission sales group identifier.</value>
    [DataMember(Order = 16)]
    public string? CommissionSalesGroupId { get; set; }

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
    [DataMember(Order = 17)]
    public DateTimeOffset Date { get; set; }

    /// <summary>
    /// Gets or sets the group identifier.
    /// </summary>
    /// <value>The group identifier.</value>
    [DataMember(Order = 13)]
    public string? GroupId { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>The name.</value>
    [DataMember(Order = 10)]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the type of the party.
    /// </summary>
    /// <value>The type of the party.</value>
    [DataMember(Order = 11)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PartyType PartyType { get; set; }

    /// <summary>
    /// Gets or sets the sales currency identifier.
    /// </summary>
    /// <value>The sales currency identifier.</value>
    [DataMember(Order = 14)]
    public string? SalesCurrencyId { get; set; }

    /// <summary>
    /// Gets or sets the warehouse identifier.
    /// </summary>
    /// <value>The warehouse identifier.</value>
    [DataMember(Order = 15)]
    public string? WarehouseId { get; set; }
}