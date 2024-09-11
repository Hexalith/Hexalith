// <copyright file="IDomainAggregateFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.Application.Aggregates;

using System;

using Hexalith.Domain.Aggregates;

/// <summary>
/// Interface IAggregateFactory.
/// </summary>
public interface IDomainAggregateFactory
{
    /// <summary>
    /// Creates the specified aggregate name.
    /// </summary>
    /// <param name="aggregateName">Name of the aggregate.</param>
    /// <returns>IAggregate.</returns>
    IDomainAggregate Create(string aggregateName);

    /// <summary>
    /// Gets the type of the aggregate.
    /// </summary>
    /// <param name="aggregateName">Name of the aggregate.</param>
    /// <returns>Type.</returns>
    Type GetAggregateType(string aggregateName);
}