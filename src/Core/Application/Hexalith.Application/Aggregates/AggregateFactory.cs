// <copyright file="AggregateFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.Application.Aggregates;

using System;

using Hexalith.Domain.Aggregates;

/// <summary>
/// Class AggregateFactory.
/// Implements the <see cref="Hexalith.Application.Aggregates.IAggregateFactory" />.
/// </summary>
/// <seealso cref="Hexalith.Application.Aggregates.IAggregateFactory" />
public class AggregateFactory : IAggregateFactory
{
    /// <summary>
    /// The aggregate providers.
    /// </summary>
    private readonly Dictionary<string, IAggregateProvider> _aggregateProviders;

    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateFactory" /> class.
    /// </summary>
    /// <param name="aggregateProviders">The aggregate providers.</param>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public AggregateFactory(IEnumerable<IAggregateProvider> aggregateProviders)
    {
        ArgumentNullException.ThrowIfNull(aggregateProviders);
        _aggregateProviders = aggregateProviders.ToDictionary(k => k.AggregateName, v => v);
    }

    /// <inheritdoc/>
    [Obsolete]
    public IAggregate Create(string aggregateName) => GetProvider(aggregateName).Create();

    /// <inheritdoc/>
    public Type GetAggregateType(string aggregateName) => GetProvider(aggregateName).AggregateType;

    private IAggregateProvider GetProvider(string aggregateName)
    {
        return _aggregateProviders.TryGetValue(aggregateName, out IAggregateProvider? value)
            ? value
            : throw new InvalidOperationException($"Provider for aggregate {aggregateName} not found in the service collection. Add the IAggregateProvider singleton to the dependency injection.");
    }
}