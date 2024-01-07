// ***********************************************************************
// Assembly         : Hexalith.Domain.Abstractions
// Author           : Jérôme Piquot
// Created          : 05-01-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 11-15-2023
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
[Serializable]
public abstract record Aggregate : IAggregate
{
    ///// <summary>
    ///// The space substitution.
    ///// </summary>
    // public const char SpaceSubstitutionCharacter = '~';

    /// <summary>
    /// Default string used for separating natural keys to compose the aggregate identifier.
    /// </summary>
    public const string Separator = "-";

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

    /// <inheritdoc/>
    public abstract bool IsInitialized();

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
    /// <exception cref="System.InvalidOperationException">The specified character '{SpaceSubstitution}' cannot be used in an aggregate identifier ({id}). It conflicts with the system's designated replacement for white spaces.</exception>
    public static string Normalize([NotNull] string id)
    {
        return string.IsNullOrWhiteSpace(id)
            ? throw new ArgumentException("The specified identifier cannot be empty or white space.", nameof(id))
            : id;

        // if (id.Contains(SpaceSubstitutionCharacter, StringComparison.OrdinalIgnoreCase))
        // {
        //    throw new InvalidOperationException($"The specified character '{SpaceSubstitutionCharacter}' cannot be used in an aggregate identifier ({id}). It conflicts with the system's designated replacement for white spaces.");
        // }

        //// replace spaces by a special character to avoid incompatibility with the space
        // return
        //    id
        //    .Replace(' ', SpaceSubstitutionCharacter);
    }
}