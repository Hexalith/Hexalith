// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-29-2023
// ***********************************************************************
// <copyright file="SalesInvoice.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.Aggregates;

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

using Hexalith.Domain.Events;
using Hexalith.Domain.Exceptions;

/// <summary>
/// Class SalesInvoice.
/// Implements the <see cref="Hexalith.Domain.Aggregates.Aggregate" />
/// Implements the <see cref="Hexalith.Domain.Aggregates.IAggregate" />.
/// </summary>
/// <seealso cref="Hexalith.Domain.Aggregates.Aggregate" />
/// <seealso cref="Hexalith.Domain.Aggregates.IAggregate" />
[DataContract]
public record SalesInvoice(
    [property: DataMember(Order = 1)] SalesInvoiceState State) : Aggregate
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SalesInvoice"/> class.
    /// </summary>
    public SalesInvoice()
        : this(new SalesInvoiceState())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SalesInvoice"/> class.
    /// </summary>
    /// <param name="issued">The issued.</param>
    public SalesInvoice(SalesInvoiceIssued issued)
        : this(new SalesInvoiceState(issued))
    {
    }

    /// <inheritdoc/>
    public override (IAggregate Aggregate, IEnumerable<BaseEvent> Events) Apply(BaseEvent domainEvent)
    {
        return (domainEvent switch
        {
            SalesInvoiceIssued issued => string.IsNullOrWhiteSpace(State.Id)
                ? new SalesInvoice(issued)
                : throw new InvalidAggregateEventException(this, domainEvent, true),
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
    /// <returns>System.String.</returns>
    public static string GetAggregateId([NotNull] string partitionId, [NotNull] string companyId, [NotNull] string originId, [NotNull] string id)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(partitionId);
        ArgumentException.ThrowIfNullOrWhiteSpace(companyId);
        ArgumentException.ThrowIfNullOrWhiteSpace(originId);
        ArgumentException.ThrowIfNullOrWhiteSpace(id);

        return Normalize(GetAggregateName() + Separator + partitionId + Separator + companyId + Separator + originId + Separator + id);
    }

    /// <inheritdoc/>
    public override bool IsInitialized() => !string.IsNullOrEmpty(State.Id);

    /// <summary>
    /// Gets the name of the aggregate.
    /// </summary>
    /// <returns>System.String.</returns>
#pragma warning disable CA1024 // Use properties where appropriate
    public static string GetAggregateName() => nameof(SalesInvoice);
#pragma warning restore CA1024 // Use properties where appropriate

    /// <inheritdoc/>
    protected override string DefaultAggregateId() => GetAggregateId(State.PartitionId, State.CompanyId, State.OriginId, State.Id);

    private void CheckEvent(SalesInvoiceEvent customerEvent, [CallerArgumentExpression(nameof(customerEvent))] string? paramName = null)
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
}