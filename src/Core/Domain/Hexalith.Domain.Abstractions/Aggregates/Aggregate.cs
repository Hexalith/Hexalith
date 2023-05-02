// ***********************************************************************
// Assembly         : Hexalith.Domain.Abstractions
// Author           : Jérôme Piquot
// Created          : 05-01-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-01-2023
// ***********************************************************************
// <copyright file="Aggregate.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.Abstractions.Aggregates;

using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Domain.Abstractions.Events;

/// <summary>
/// Class Aggregate.
/// Implements the <see cref="Hexalith.Domain.Abstractions.Aggregates.IAggregate" />.
/// </summary>
/// <seealso cref="Hexalith.Domain.Abstractions.Aggregates.IAggregate" />
[DataContract]
[DebuggerDisplay("{AggregateName}/{AggregateId}")]
public abstract record Aggregate : IAggregate
{
    /// <summary>
    /// Default string used for separating natural keys to compose the aggregate identifier.
    /// </summary>
    protected const string Separator = "-";

    /// <inheritdoc/>
    [IgnoreDataMember]
    [JsonIgnore]
    public string AggregateId => DefaultAggregateId();

    /// <inheritdoc/>
    [IgnoreDataMember]
    [JsonIgnore]
    public string AggregateName => DefaultAggregateName();

    /// <inheritdoc/>
    public abstract IAggregate Apply(BaseEvent domainEvent);

    /// <summary>
    /// Get the aggregate identifier.
    /// </summary>
    /// <returns>The identifier.</returns>
    protected abstract string DefaultAggregateId();

    /// <summary>
    /// Get the aggregate name.
    /// </summary>
    /// <returns>The name.</returns>
    protected virtual string DefaultAggregateName() => GetType().Name;
}