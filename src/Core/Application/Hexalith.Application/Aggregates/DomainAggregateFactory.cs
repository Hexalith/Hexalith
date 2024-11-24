// <copyright file="DomainAggregateFactory.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Aggregates;

using System;

using Hexalith.Domain.Aggregates;

/// <summary>
/// Class AggregateFactory.
/// Implements the <see cref="Hexalith.Application.Aggregates.IAggregateFactory" />.
/// </summary>
/// <seealso cref="Hexalith.Application.Aggregates.IAggregateFactory" />
public class DomainAggregateFactory : IDomainAggregateFactory
{
    /// <summary>
    /// The aggregate providers.
    /// </summary>
    private readonly Dictionary<string, IDomainAggregateProvider> _aggregateProviders;

    /// <summary>
    /// Initializes a new instance of the <see cref="DomainAggregateFactory"/> class.
    /// </summary>
    /// <param name="aggregateProviders">The aggregate providers.</param>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public DomainAggregateFactory(IEnumerable<IDomainAggregateProvider> aggregateProviders)
    {
        ArgumentNullException.ThrowIfNull(aggregateProviders);
        _aggregateProviders = aggregateProviders.ToDictionary(k => k.AggregateName, v => v);
    }

    /// <inheritdoc/>
    public IDomainAggregate Create(string aggregateName) => GetProvider(aggregateName).Create();

    /// <inheritdoc/>
    public Type GetAggregateType(string aggregateName) => GetProvider(aggregateName).AggregateType;

    private IDomainAggregateProvider GetProvider(string aggregateName)
    {
        return _aggregateProviders.TryGetValue(aggregateName, out IDomainAggregateProvider? value)
            ? value
            : throw new InvalidOperationException($"Provider for aggregate {aggregateName} not found in the service collection. Add the IDomainAggregateProvider singleton to the dependency injection.");
    }
}