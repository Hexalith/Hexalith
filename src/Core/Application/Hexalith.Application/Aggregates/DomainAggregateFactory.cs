// <copyright file="DomainAggregateFactory.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Aggregates;

using System;
using System.Diagnostics.CodeAnalysis;

using Hexalith.Domain.Aggregates;

/// <summary>
/// Factory class for creating domain aggregates.
/// </summary>
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
    /// <exception cref="System.ArgumentNullException">Thrown when aggregateProviders is null.</exception>
    public DomainAggregateFactory(IEnumerable<IDomainAggregateProvider> aggregateProviders)
    {
        ArgumentNullException.ThrowIfNull(aggregateProviders);
        _aggregateProviders = aggregateProviders.ToDictionary(k => k.AggregateName, v => v);
    }

    /// <inheritdoc/>
    public IDomainAggregate Create(string aggregateName) => GetProvider(aggregateName).Create();

    /// <inheritdoc/>
    public Type GetAggregateType(string aggregateName) => GetProvider(aggregateName).AggregateType;

    /// <summary>
    /// Gets the provider for the specified aggregate name.
    /// </summary>
    /// <param name="aggregateName">The name of the aggregate.</param>
    /// <returns>The provider for the specified aggregate name.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the provider for the specified aggregate name is not found.</exception>
    private IDomainAggregateProvider GetProvider([NotNull] string aggregateName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(aggregateName);
        bool provider = _aggregateProviders.TryGetValue(aggregateName, out IDomainAggregateProvider? value);
        if (!provider)
        {
            string existingProviders = string.Join(", ", _aggregateProviders.Keys);
            throw new InvalidOperationException($"Provider for aggregate {aggregateName} not found in the service collection. Add the IDomainAggregateProvider singleton to the dependency injection. Found providers : " + existingProviders);
        }

        return value;
    }
}