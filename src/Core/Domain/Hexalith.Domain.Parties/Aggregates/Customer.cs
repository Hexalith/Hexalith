// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-29-2023
// ***********************************************************************
// <copyright file="Customer.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

/// <summary>
/// The Aggregates namespace.
/// </summary>
namespace Hexalith.Domain.Aggregates;

using Hexalith.Domain.Events;
using Hexalith.Domain.Exceptions;
using Hexalith.Domain.ValueObjets;

/// <summary>
/// Class Customer.
/// Implements the <see cref="Hexalith.Domain.Aggregates.Aggregate" />
/// Implements the <see cref="Hexalith.Domain.Aggregates.IAggregate" />
/// Implements the <see cref="System.IEquatable{Hexalith.Domain.Aggregates.Aggregate}" />
/// Implements the <see cref="System.IEquatable{Hexalith.Domain.Aggregates.Customer}" />.
/// </summary>
/// <seealso cref="Hexalith.Domain.Aggregates.Aggregate" />
/// <seealso cref="Hexalith.Domain.Aggregates.IAggregate" />
/// <seealso cref="System.IEquatable{Hexalith.Domain.Aggregates.Aggregate}" />
/// <seealso cref="System.IEquatable{Hexalith.Domain.Aggregates.Customer}" />
public record Customer(
    string Id,
    string Name,
    string CompanyId,
    Contact Contact,
    PostalAddress InvoiceAddress,
    PostalAddress DeliveryAddress,
    string? WarehouseId,
    string? CommissionSalesGroupId,
    DateTimeOffset Date) : Aggregate
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Customer" /> class.
    /// </summary>
    /// <param name="customer">The customer.</param>
    public Customer(CustomerRegistered customer)
        : this(
              customer.Id,
              customer.Name,
              customer.CompanyId,
              customer.Contact,
              customer.InvoiceAddress,
              customer.DeliveryAddress,
              customer.WarehouseId,
              customer.CommissionSalesGroupId,
              customer.Date)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Customer" /> class.
    /// </summary>
    /// <param name="customer">The customer.</param>
    public Customer(CustomerInformationChanged customer)
        : this(
              customer.Id,
              customer.Name,
              customer.CompanyId,
              customer.Contact,
              customer.InvoiceAddress,
              customer.DeliveryAddress,
              customer.WarehouseId,
              customer.CommissionSalesGroupId,
              customer.Date)
    {
    }

    /// <inheritdoc/>
    public override IAggregate Apply(BaseEvent domainEvent)
    {
        return domainEvent switch
        {
            CustomerInformationChanged changed => new Customer(changed),
            CustomerRegistered => throw new InvalidAggregateEventException(this, domainEvent, true),
            _ => throw new InvalidAggregateEventException(this, domainEvent, false),
        };
    }

    /// <inheritdoc/>
    protected override string DefaultAggregateId() => base.DefaultAggregateId() + Separator + Id;
}