// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-18-2024
// ***********************************************************************
// <copyright file="InventoryUnit.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.InventoryUnits.Aggregates;

using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Events;
using Hexalith.Domain.Exceptions;
using Hexalith.Domain.InventoryUnits.Events;

/// <summary>
/// Class InventoryUnitConversion.
/// Implements the <see cref="Aggregate" />
/// Implements the <see cref="IAggregate" />
/// Implements the <see cref="IEquatable{Aggregate}" />
/// Implements the <see cref="IEquatable{InventoryUnitConversion}" />.
/// </summary>
/// <seealso cref="Aggregate" />
/// <seealso cref="IAggregate" />
/// <seealso cref="IEquatable{Aggregate}" />
/// <seealso cref="IEquatable{InventoryUnitConversion}" />
public record InventoryUnit(
    string PartitionId,
    string CompanyId,
    string OriginId,
    string Id,
    string Name,
    string? Description,
    int RoundDecimals) : Aggregate
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryUnit" /> class.
    /// </summary>
    /// <param name="unit">The InventoryUnitConversion.</param>
    public InventoryUnit(InventoryUnitAdded unit)
        : this(
              (unit ?? throw new ArgumentNullException(nameof(unit))).PartitionId,
              unit.CompanyId,
              unit.OriginId,
              unit.Id,
              unit.Name,
              unit.Description,
              unit.RoundDecimals)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryUnit"/> class.
    /// </summary>
    public InventoryUnit()
        : this(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 0)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryUnit" /> class.
    /// </summary>
    /// <param name="unit">The InventoryUnitConversion.</param>
    public InventoryUnit(InventoryUnitChanged unit)
        : this(
              (unit ?? throw new ArgumentNullException(nameof(unit))).PartitionId,
              unit.CompanyId,
              unit.OriginId,
              unit.Id,
              unit.Name,
              unit.Description,
              unit.RoundDecimals)
    {
    }

    /// <inheritdoc/>
    public override (IAggregate Aggregate, IEnumerable<BaseEvent> Events) Apply(BaseEvent domainEvent)
    {
        return (domainEvent switch
        {
            InventoryUnitChanged changed => new InventoryUnit(changed),
            InventoryUnitAdded => throw new InvalidAggregateEventException(this, domainEvent, true),
            _ => throw new InvalidAggregateEventException(this, domainEvent, false),
        }, []);
    }

    /// <summary>
    /// Gets the aggregate identifier.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="originId">The origin identifier.</param>
    /// <param name="id">The identifier.</param>
    /// <returns>string.</returns>
    public static string GetAggregateId(
        string partitionId,
        string companyId,
        string originId,
        string id)
        => Normalize(GetAggregateName() +
            Separator + partitionId +
            Separator + companyId +
            Separator + originId +
            Separator + id);

    /// <summary>
    /// Gets the name of the aggregate.
    /// </summary>
    /// <returns>System.String.</returns>
    public static string GetAggregateName() => nameof(InventoryUnit);

    /// <inheritdoc/>
    public override bool IsInitialized() => !string.IsNullOrWhiteSpace(Id);

    /// <inheritdoc/>
    protected override string DefaultAggregateId() => GetAggregateId(PartitionId, CompanyId, OriginId, Id);
}