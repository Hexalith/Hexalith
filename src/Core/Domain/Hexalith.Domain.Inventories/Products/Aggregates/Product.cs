// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-29-2023
// ***********************************************************************
// <copyright file="Product.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.Products.Aggregates;

using System.Runtime.Serialization;

using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Events;
using Hexalith.Domain.Exceptions;
using Hexalith.Domain.Products.Events;
using Hexalith.Domain.Products.ValueObjects;

/// <summary>
/// Class InventoryItem.
/// Implements the <see cref="Aggregate" />
/// Implements the <see cref="IAggregate" />
/// Implements the <see cref="IEquatable{Aggregate}" />
/// Implements the <see cref="IEquatable{InventoryItem}" />.
/// </summary>
/// <seealso cref="Aggregate" />
/// <seealso cref="IAggregate" />
/// <seealso cref="IEquatable{Aggregate}" />
/// <seealso cref="IEquatable{InventoryItem}" />
[DataContract]
public record Product(
    [property: DataMember] string PartitionId,
    [property: DataMember] string OriginId,
    [property: DataMember] string Id,
    [property: DataMember] string Name,
    [property: DataMember] string? Description,
    [property: DataMember] bool Disabled,
    [property: DataMember] IEnumerable<ProductDimension>? Dimensions,
    [property: DataMember] IEnumerable<IEnumerable<string>>? ExcludedDimensionCombinaisons) : CommonEntityAggregate(PartitionId, OriginId, Id)
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Product" /> class.
    /// </summary>
    /// <param name="added">The InventoryItem.</param>
    public Product(ProductAdded added)
        : this(
              (added ?? throw new ArgumentNullException(nameof(added))).PartitionId,
              added.OriginId,
              added.Id,
              added.Name,
              added.Description,
              false,
              null,
              null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Product"/> class.
    /// </summary>
    public Product()
        : this(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, false, null, null)
    {
    }

    /// <inheritdoc/>
    public override (IAggregate Aggregate, IEnumerable<BaseEvent> Events) Apply(BaseEvent domainEvent)
    {
        return (domainEvent switch
        {
            ProductEnabled => this with { Disabled = false },
            ProductDisabled => this with { Disabled = true },
            ProductDescriptionChanged changed => this with { Name = changed.Name, Description = changed.Name },
            ProductDimensionsChanged changed => this with
            {
                Dimensions = changed.ProductDimensions,
                ExcludedDimensionCombinaisons = changed.ExcludedDimensionCombinaisons,
            },
            ProductAdded => throw new InvalidAggregateEventException(this, domainEvent, true),
            _ => throw new InvalidAggregateEventException(this, domainEvent, false),
        }, [domainEvent]);
    }

    /// <summary>
    /// Gets the aggregate identifier.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="originId">The origin identifier.</param>
    /// <param name="id">The identifier.</param>
    /// <returns>string.</returns>
    public static string GetAggregateId(string partitionId, string originId, string id)
        => Normalize(GetAggregateName() + Separator + partitionId + Separator + originId + Separator + id);

    /// <summary>
    /// Gets the name of the aggregate.
    /// </summary>
    /// <returns>System.String.</returns>
    public static string GetAggregateName() => nameof(Product);

    /// <inheritdoc/>
    public override bool IsInitialized() => !string.IsNullOrWhiteSpace(Id);

    /// <inheritdoc/>
    protected override string DefaultAggregateId() => GetAggregateId(PartitionId, OriginId, Id);
}