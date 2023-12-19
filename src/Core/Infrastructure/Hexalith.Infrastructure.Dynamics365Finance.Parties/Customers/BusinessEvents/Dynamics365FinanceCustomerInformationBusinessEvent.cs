// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Dynamics365Finance.Parties
// Author           : Jérôme Piquot
// Created          : 01-02-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 12-07-2023
// ***********************************************************************
// <copyright file="Dynamics365FinanceCustomerInformationBusinessEvent.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.BusinessEvents;

using System.Collections.Generic;
using System.Runtime.Serialization;

using Hexalith.Domain.Aggregates;
using Hexalith.Domain.ValueObjets;
using Hexalith.Infrastructure.Dynamics365Finance.BusinessEvents;

/// <summary>
/// This is the base class for logistics partner catalog events from Dynamics 365 for finance and operations.
/// </summary>
[DataContract]
public abstract class Dynamics365FinanceCustomerInformationBusinessEvent : Dynamics365BusinessEventBase
{
    /// <summary>
    /// Gets or sets the account.
    /// </summary>
    /// <value>The account.</value>
    [DataMember]
    public string? Account { get; set; }

    /// <inheritdoc/>
    public override string AggregateId =>
        AggregateName + Aggregate.Separator +
        PartitionId + Aggregate.Separator +
        BusinessEventLegalEntity?.ToUpperInvariant() + Aggregate.Separator +
        OriginId + Aggregate.Separator +
        Account;

    /// <inheritdoc/>
    public override string AggregateName => nameof(Dynamics365Finance) + Customer.GetAggregateName();

    /// <summary>
    /// Gets or sets the commission sales group identifier.
    /// </summary>
    /// <value>The commission sales group identifier.</value>
    [DataMember]
    public string? CommissionSalesGroupId { get; set; }

    /// <summary>
    /// Gets or sets the primary contact.
    /// </summary>
    /// <value>The primary contact.</value>
    [DataMember]
    public Contact? Contact { get; set; }

    /// <summary>
    /// Gets or sets the external references.
    /// </summary>
    /// <value>The external references.</value>
    [DataMember]
    public IEnumerable<ExternalReference>? ExternalReferences { get; set; }

    /// <summary>
    /// Gets or sets the group identifier.
    /// </summary>
    /// <value>The group identifier.</value>
    [DataMember]
    public string? GroupId { get; set; }

    /// <summary>
    /// Gets or sets the inter company direct delivery.
    /// </summary>
    /// <value>The inter company direct delivery.</value>
    [DataMember]
    public string? InterCompanyDirectDelivery { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>The name.</value>
    [DataMember]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the type of the party.
    /// </summary>
    /// <value>The type of the party.</value>
    [DataMember]
    public string? PartyType { get; set; }

    /// <summary>
    /// Gets or sets the sales currency identifier.
    /// </summary>
    /// <value>The sales currency identifier.</value>
    [DataMember]
    public string? SalesCurrencyId { get; set; }

    /// <summary>
    /// Gets or sets the warehouse identifier.
    /// </summary>
    /// <value>The warehouse identifier.</value>
    [DataMember]
    public string? WarehouseId { get; set; }
}