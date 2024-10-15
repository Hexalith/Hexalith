// <copyright file="IAggregateProvider.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Aggregates;

using Hexalith.Domain.Aggregates;

/// <summary>
/// Interface IAggregateProvider.
/// </summary>
[Obsolete("This interface is not used anymore. Use IDomainAggregateProvider instead.", true)]
public interface IAggregateProvider
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