// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-21-2023
// ***********************************************************************
// <copyright file="AggregateExternalReference.cs" company="Fiveforty SAS Paris France">
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
using Hexalith.Domain.ValueObjets;

/// <summary>
/// Class Customer.
/// Implements the <see cref="Hexalith.Domain.Aggregates.Aggregate" />
/// Implements the <see cref="Hexalith.Domain.Aggregates.IAggregate" />
/// Implements the <see cref="System.IEquatable{Hexalith.Domain.Aggregates.Aggregate}" />
/// Implements the <see cref="System.IEquatable{Hexalith.Domain.Aggregates.AggregateExternalReference}" />.
/// </summary>
/// <seealso cref="Hexalith.Domain.Aggregates.Aggregate" />
/// <seealso cref="Hexalith.Domain.Aggregates.IAggregate" />
/// <seealso cref="System.IEquatable{Hexalith.Domain.Aggregates.Aggregate}" />
/// <seealso cref="System.IEquatable{Hexalith.Domain.Aggregates.AggregateExternalReference}" />
[DataContract]
public record AggregateExternalReference(
    string Id,
    IEnumerable<ExternalReference> ExternalIds) : Aggregate
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateExternalReference" /> class.
    /// </summary>
    /// <param name="mapped">The mapped.</param>
    public AggregateExternalReference(AggregateExternalReferenceAdded mapped)
        : this(mapped.Id, new ValueObjets.ExternalReference[] { new ValueObjets.ExternalReference(mapped.SystemId, mapped.ExternalId) })
    {
    }

    /// <inheritdoc/>
    public override IAggregate Apply(BaseEvent domainEvent)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);
        return domainEvent switch
        {
            AggregateExternalReferenceRemoved removed => this with
            { ExternalIds = ExternalIds.Where(p => p.SystemId != removed.SystemId).ToList() },
            AggregateExternalReferenceAdded added => this with
            {
                ExternalIds = ExternalIds
                .Union(new ExternalReference[] { new ExternalReference(added.SystemId, added.ExternalId) })

                // Convert to dictionary to check duplicate keys
                .ToDictionary(k => k.SystemId, v => v)
                .Values
                .ToList(),
            },
            _ => throw new InvalidAggregateEventException(this, domainEvent, false),
        };
    }

    /// <inheritdoc/>
    protected override string DefaultAggregateId() => base.DefaultAggregateId() + Separator + Id;
}