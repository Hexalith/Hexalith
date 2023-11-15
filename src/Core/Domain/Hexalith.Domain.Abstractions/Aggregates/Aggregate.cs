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

namespace Hexalith.Domain.Aggregates;

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Domain.Events;

/// <summary>
/// Class Aggregate.
/// Implements the <see cref="IAggregate" />.
/// </summary>
/// <seealso cref="IAggregate" />
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
    protected virtual string DefaultAggregateId() => DefaultAggregateName();

    /// <summary>
    /// Get the aggregate name.
    /// </summary>
    /// <returns>The name.</returns>
    protected virtual string DefaultAggregateName() => GetType().Name;

    /// <summary>
    /// Normalizes the specified identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>System.String.</returns>
    protected static string Normalize([NotNull] string id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);

        // replace spaces by tilde and escape tilde by double tilde
        return
            id
            .Replace("~", "~~", StringComparison.OrdinalIgnoreCase)
            .Replace(" ", "~", StringComparison.OrdinalIgnoreCase);
    }
}