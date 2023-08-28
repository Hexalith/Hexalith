// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-28-2023
// ***********************************************************************
// <copyright file="ExternalSystemReference.cs" company="Fiveforty SAS Paris France">
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

using System.Runtime.Serialization;

using Hexalith.Domain.Events;
using Hexalith.Domain.Exceptions;

/// <summary>
/// Class Customer.
/// Implements the <see cref="Hexalith.Domain.Aggregates.Aggregate" />
/// Implements the <see cref="Hexalith.Domain.Aggregates.IAggregate" />
/// Implements the <see cref="System.IEquatable{Hexalith.Domain.Aggregates.Aggregate}" />
/// Implements the <see cref="System.IEquatable{Hexalith.Domain.Aggregates.ExternalSystemReference}" />.
/// </summary>
/// <seealso cref="Hexalith.Domain.Aggregates.Aggregate" />
/// <seealso cref="Hexalith.Domain.Aggregates.IAggregate" />
/// <seealso cref="System.IEquatable{Hexalith.Domain.Aggregates.Aggregate}" />
/// <seealso cref="System.IEquatable{Hexalith.Domain.Aggregates.ExternalSystemReference}" />
[DataContract]
public record ExternalSystemReference(
    string SystemId,
    string ExternalId,
    string ReferenceAggregateName,
    string? ReferenceAggregateId) : Aggregate
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalSystemReference" /> class.
    /// </summary>
    /// <param name="mapped">The mapped.</param>
    public ExternalSystemReference(ExternalSystemReferenceMapped mapped)
        : this(mapped.SystemId, mapped.ExternalId, mapped.ReferenceAggregateName, mapped.Id)
    {
    }

    /// <inheritdoc/>
    public override IAggregate Apply(BaseEvent domainEvent)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);
        return domainEvent switch
        {
            ExternalSystemReferenceUnmapped => this with { ReferenceAggregateId = null },
            ExternalSystemReferenceMapped added => this with { ReferenceAggregateId = added.Id },
            _ => throw new InvalidAggregateEventException(this, domainEvent, false),
        };
    }

    /// <inheritdoc/>
    protected override string DefaultAggregateId() => nameof(ExternalSystemReference) + Separator + SystemId + Separator + ReferenceAggregateName + Separator + ExternalId;
}