// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 09-12-2023
// ***********************************************************************
// <copyright file="InventoryItemStock.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.Aggregates;

using Hexalith.Domain.Events;
using Hexalith.Domain.Exceptions;

/// <summary>
/// Class InventoryItemStock.
/// Implements the <see cref="Hexalith.Domain.Aggregates.Aggregate" />
/// Implements the <see cref="Hexalith.Domain.Aggregates.IAggregate" />
/// Implements the <see cref="System.IEquatable{Hexalith.Domain.Aggregates.Aggregate}" />
/// Implements the <see cref="System.IEquatable{Hexalith.Domain.Aggregates.InventoryItemStock}" />.
/// </summary>
/// <seealso cref="Hexalith.Domain.Aggregates.Aggregate" />
/// <seealso cref="Hexalith.Domain.Aggregates.IAggregate" />
/// <seealso cref="System.IEquatable{Hexalith.Domain.Aggregates.Aggregate}" />
/// <seealso cref="System.IEquatable{Hexalith.Domain.Aggregates.InventoryItemStock}" />
public record InventoryItemStock(
    string PartitionId,
    string CompanyId,
    string OriginId,
    string LocationId,
    string Id,
    decimal Quantity,
    DateTimeOffset Date) : Aggregate
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryItemStock" /> class.
    /// </summary>
    /// <param name="increased">The increased.</param>
    public InventoryItemStock(InventoryItemStockIncreased increased)
        : this(
              (increased ?? throw new ArgumentNullException(nameof(increased))).PartitionId,
              increased.OriginId,
              increased.CompanyId,
              increased.LocationId,
              increased.Id,
              increased.Quantity,
              increased.Date)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryItemStock" /> class.
    /// </summary>
    /// <param name="decreased">The decreased.</param>
    public InventoryItemStock(InventoryItemStockDecreased decreased)
        : this(
              (decreased ?? throw new ArgumentNullException(nameof(decreased))).PartitionId,
              decreased.OriginId,
              decreased.CompanyId,
              decreased.LocationId,
              decreased.Id,
              -decreased.Quantity,
              decreased.Date)
    {
    }

    /// <inheritdoc/>
    public override IAggregate Apply(BaseEvent domainEvent)
    {
        return domainEvent switch
        {
            InventoryItemStockIncreased increased => this with { Quantity = Quantity + increased.Quantity },
            InventoryItemStockDecreased decreased => this with { Quantity = Quantity - decreased.Quantity },
            _ => throw new InvalidAggregateEventException(this, domainEvent, false),
        };
    }

    /// <inheritdoc/>
    protected override string DefaultAggregateId()
        => GetAggregateId(PartitionId, OriginId, CompanyId, LocationId, Id);

    /// <summary>
    /// Gets the aggregate identifier.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="originId">The origin identifier.</param>
    /// <param name="locationId">The location identifier.</param>
    /// <param name="id">The identifier.</param>
    /// <returns>System.String.</returns>
    public static string GetAggregateId(string partitionId, string companyId, string originId, string locationId, string id)
        => Normalize(GetAggregateName() + Separator + partitionId + Separator + companyId + Separator + originId + Separator + locationId + Separator + id);

    /// <summary>
    /// Gets the name of the aggregate.
    /// </summary>
    /// <returns>System.String.</returns>
#pragma warning disable CA1024 // Use properties where appropriate
    public static string GetAggregateName() => nameof(InventoryItemStock);
#pragma warning restore CA1024 // Use properties where appropriate
}