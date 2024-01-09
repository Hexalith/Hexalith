// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-21-2023
// ***********************************************************************
// <copyright file="ExternalCustomer.cs" company="Fiveforty SAS Paris France">
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
/// Implements the <see cref="System.IEquatable{Hexalith.Domain.Aggregates.ExternalCustomer}" />.
/// </summary>
/// <seealso cref="Hexalith.Domain.Aggregates.Aggregate" />
/// <seealso cref="Hexalith.Domain.Aggregates.IAggregate" />
/// <seealso cref="System.IEquatable{Hexalith.Domain.Aggregates.Aggregate}" />
/// <seealso cref="System.IEquatable{Hexalith.Domain.Aggregates.ExternalCustomer}" />
[DataContract]
public record ExternalCustomer(
    string System,
    string ExternalId,
    string Id) : Aggregate
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalCustomer" /> class.
    /// </summary>
    /// <param name="mapped">The mapped.</param>
    public ExternalCustomer(ExternalCustomerMapped mapped)
        : this(mapped.System, mapped.ExternalId, mapped.Id)
    {
    }

    /// <inheritdoc/>
    public override IAggregate Apply(BaseEvent domainEvent)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);
        return domainEvent switch
        {
            ExternalCustomerDetached => this with { Id = string.Empty },
            ExternalCustomerMapped => throw new InvalidAggregateEventException(this, domainEvent, true),
            _ => throw new InvalidAggregateEventException(this, domainEvent, false),
        };
    }

    /// <inheritdoc/>
    protected override string DefaultAggregateId() => nameof(ExternalCustomer) + Separator + System + Separator + ExternalId;
    /// <inheritdoc/>
    public override bool IsInitialized() => !string.IsNullOrWhiteSpace(Id);
}