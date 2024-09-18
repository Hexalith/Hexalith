// <copyright file="IDomainAggregateProvider.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.Application.Aggregates;

using Hexalith.Domain.Aggregates;

/// <summary>
/// Interface IDomainAggregateProvider.
/// </summary>
public interface IDomainAggregateProvider
{
    /// <summary>
    /// Gets the name of the aggregate.
    /// </summary>
    /// <value>The name of the aggregate.</value>
    string AggregateName { get; }

    /// <summary>
    /// Gets the type of the aggregate.
    /// </summary>
    /// <value>The type of the aggregate.</value>
    Type AggregateType { get; }

    /// <summary>
    /// Creates this instance.
    /// </summary>
    /// <returns>IAggregate.</returns>
    IDomainAggregate Create();
}