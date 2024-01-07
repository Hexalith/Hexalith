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

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

using Hexalith.Domain.Events;
using Hexalith.Domain.Exceptions;
using Hexalith.Domain.ValueObjets;
using Hexalith.Extensions;

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
[DataContract]
public record Customer(
    string PartitionId,
    string CompanyId,
    string OriginId,
    string Id,
    string Name,
    PartyType PartyType,
    Contact Contact,
    string? WarehouseId,
    string? CommissionSalesGroupId,
    string? GroupId,
    string? SalesCurrencyId,
    bool IntercompanyDropship,
    DateTimeOffset Date) : Aggregate
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Customer"/> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    public Customer()
        : this(
              string.Empty,
              string.Empty,
              string.Empty,
              string.Empty,
              string.Empty,
              PartyType.Other,
              new Contact(),
              null,
              null,
              null,
              null,
              false,
              DateTimeOffset.MinValue)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Customer" /> class.
    /// </summary>
    /// <param name="customer">The customer.</param>
    public Customer(CustomerRegistered customer)
        : this(
              (customer ?? throw new ArgumentNullException(nameof(customer))).PartitionId,
              customer.CompanyId,
              customer.OriginId,
              customer.Id,
              customer.Name,
              customer.PartyType,
              new Contact(customer.Contact),
              customer.WarehouseId,
              customer.CommissionSalesGroupId,
              customer.GroupId,
              customer.SalesCurrencyId,
              false,
              customer.Date)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Customer"/> class.
    /// </summary>
    /// <param name="customer">The customer.</param>
    /// <param name="intercompanyDropship">if set to <c>true</c> [intercompany dropship].</param>
    public Customer(CustomerRegistered customer, bool intercompanyDropship)
        : this(customer) => IntercompanyDropship = intercompanyDropship;

    /// <inheritdoc/>
    public override IAggregate Apply(BaseEvent domainEvent)
    {
        return domainEvent switch
        {
            CustomerInformationChanged changed => this with
            {
                Name = changed.Name,
                Contact = new Contact(changed.Contact),
                WarehouseId = changed.WarehouseId,
                CommissionSalesGroupId = changed.CommissionSalesGroupId,
                Date = changed.Date,
                PartyType = changed.PartyType,
                GroupId = changed.GroupId,
                SalesCurrencyId = changed.SalesCurrencyId,
            },
            IntercompanyDropshipDeliveryForCustomerSelected => this with { IntercompanyDropship = true },
            IntercompanyDropshipDeliveryForCustomerDeselected => this with { IntercompanyDropship = false },
            CustomerRegistered => throw new InvalidAggregateEventException(this, domainEvent, true),
            _ => throw new InvalidAggregateEventException(this, domainEvent, false),
        };
    }

    /// <inheritdoc/>
    protected override string DefaultAggregateId() => GetAggregateId(PartitionId, CompanyId, OriginId, Id);

    /// <summary>
    /// Gets the aggregate identifier.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="originId">The origin identifier.</param>
    /// <param name="id">The identifier.</param>
    /// <returns>System.String.</returns>
    public static string GetAggregateId([NotNull] string partitionId, [NotNull] string companyId, [NotNull] string originId, [NotNull] string id)
    {
        ArgumentException.ThrowIfNullOrEmpty(partitionId);
        ArgumentException.ThrowIfNullOrEmpty(companyId);
        ArgumentException.ThrowIfNullOrEmpty(originId);
        ArgumentException.ThrowIfNullOrEmpty(id);

        return Normalize(GetAggregateName() + Separator + partitionId + Separator + companyId + Separator + originId + Separator + id);
    }

    /// <summary>
    /// Gets the name of the aggregate.
    /// </summary>
    /// <returns>System.String.</returns>
#pragma warning disable CA1024 // Use properties where appropriate
    public static string GetAggregateName() => nameof(Customer);
#pragma warning restore CA1024 // Use properties where appropriate

    private void CheckEvent(CustomerEvent customerEvent, [CallerArgumentExpression(nameof(customerEvent))] string? paramName = null)
    {
        if (AggregateName != customerEvent.AggregateName)
        {
            throw new ArgumentException($"{customerEvent.TypeName} can not be applied to aggregate {AggregateName}.", paramName);
        }

        if (AggregateId != customerEvent.AggregateId)
        {
            throw new ArgumentException($"{customerEvent.TypeName} aggregate aggregate Id '{customerEvent.AggregateId}' is invalid. Expected : '{AggregateId}'.", paramName);
        }
    }

    /// <summary>
    /// Converts to change customer information event.
    /// </summary>
    /// <returns>CustomerInformationChanged.</returns>
    public CustomerInformationChanged ToCustomerInformationChanged()
    {
        return new CustomerInformationChanged(
            PartitionId,
            CompanyId,
            OriginId,
            Id,
            Name,
            PartyType,
            new Contact(Contact),
            WarehouseId,
            CommissionSalesGroupId,
            GroupId,
            SalesCurrencyId,
            Date);
    }

    /// <summary>
    /// Converts to register customer event.
    /// </summary>
    /// <returns>CustomerRegistered.</returns>
    public CustomerRegistered ToCustomerRegistered()
    {
        return new CustomerRegistered(
            PartitionId,
            CompanyId,
            OriginId,
            Id,
            Name,
            PartyType,
            new Contact(Contact),
            WarehouseId,
            CommissionSalesGroupId,
            GroupId,
            SalesCurrencyId,
            Date);
    }

    /// <inheritdoc/>
    public override bool IsInitialized() => !string.IsNullOrWhiteSpace(Id);
}