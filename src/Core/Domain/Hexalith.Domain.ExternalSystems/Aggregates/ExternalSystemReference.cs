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

namespace Hexalith.Domain.Aggregates;

using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

using Hexalith.Domain.Events;
using Hexalith.Domain.Exceptions;

/// <summary>
/// Class Customer.
/// Implements the <see cref="Hexalith.Domain.Aggregates.Aggregate" />
/// Implements the <see cref="Hexalith.Domain.Aggregates.IAggregate" />.
/// </summary>
/// <seealso cref="Hexalith.Domain.Aggregates.Aggregate" />
/// <seealso cref="Hexalith.Domain.Aggregates.IAggregate" />
[DataContract]
public record ExternalSystemReference(
    [property: DataMember] string PartitionId,
    [property: DataMember] string CompanyId,
    [property: DataMember] string SystemId,
    [property: DataMember] string ReferenceAggregateName,
    [property: DataMember] string ExternalId,
    [property: DataMember] string? ReferenceAggregateId) : Aggregate
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalSystemReference" /> class.
    /// </summary>
    /// <param name="mapped">The mapped.</param>
    public ExternalSystemReference(ExternalSystemReferenceAdded mapped)
        : this(
              (mapped ?? throw new ArgumentNullException(nameof(mapped))).PartitionId,
              mapped.CompanyId,
              mapped.SystemId,
              mapped.ReferenceAggregateName,
              mapped.ExternalId,
              mapped.ReferenceAggregateId)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalSystemReference"/> class.
    /// </summary>
    public ExternalSystemReference()
        : this(
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty)
    {
    }

    /// <inheritdoc/>
    public override (IAggregate Aggregate, IEnumerable<BaseEvent> Events) Apply(BaseEvent domainEvent)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);
        return (domainEvent switch
        {
            ExternalSystemReferenceRemoved => this with { ReferenceAggregateId = null },
            ExternalSystemReferenceAdded added => this with { ReferenceAggregateId = added.ReferenceAggregateId },
            _ => throw new InvalidAggregateEventException(this, domainEvent, false),
        }, []);
    }

    /// <summary>
    /// Gets the aggregate identifier.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="systemId">The system identifier.</param>
    /// <param name="referenceAggregateName">Name of the reference aggregate.</param>
    /// <param name="externalId">The external identifier.</param>
    /// <returns>System.String.</returns>
    public static string GetAggregateId(
        [NotNull] string partitionId,
        [NotNull] string companyId,
        [NotNull] string systemId,
        [NotNull] string referenceAggregateName,
        [NotNull] string externalId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(partitionId);
        ArgumentException.ThrowIfNullOrWhiteSpace(companyId);
        ArgumentException.ThrowIfNullOrWhiteSpace(systemId);
        ArgumentException.ThrowIfNullOrWhiteSpace(referenceAggregateName);
        ArgumentException.ThrowIfNullOrWhiteSpace(externalId);
        return Normalize(GetAggregateName() + Separator + partitionId + Separator + companyId + Separator + systemId + Separator + referenceAggregateName + Separator + externalId);
    }

    /// <summary>
    /// Gets the name of the aggregate.
    /// </summary>
    /// <returns>System.String.</returns>
#pragma warning disable CA1024 // Use properties where appropriate
    public static string GetAggregateName() => nameof(ExternalSystemReference);

#pragma warning restore CA1024 // Use properties where appropriate
    /// <inheritdoc/>
    public override bool IsInitialized() => !string.IsNullOrWhiteSpace(ReferenceAggregateName);

    /// <inheritdoc/>
    protected override string DefaultAggregateId() => GetAggregateId(PartitionId, CompanyId, SystemId, ReferenceAggregateName, ExternalId);
}