// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-21-2023
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

using System.Runtime.Serialization;

using Hexalith.Domain.Events;
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
[DataContract]
public record Customer(string Id, string Name, Contact Contact, PostalAddress InvoiceAddress, PostalAddress DeliveryAddress, Dictionary<string, string> ExternalIds) : Aggregate
{
    /// <inheritdoc/>
    public override IAggregate Apply(BaseEvent domainEvent) => throw new NotImplementedException();

    /// <inheritdoc/>
    protected override string DefaultAggregateId() => nameof(Customer) + Separator + Id;
}