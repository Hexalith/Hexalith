// <copyright file="IDomainAggregateProvider.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Aggregates;

using Hexalith.Domains;

/// <summary>
/// Interface IDomainAggregateProvider.
/// </summary>
public interface IDomainAggregateProvider
{
    /// <summary>
    /// Gets the type of the aggregate.
    /// </summary>
    /// <value>The type of the aggregate.</value>
    Type AggregateType { get; }

    /// <summary>
    /// Gets the name of the aggregate.
    /// </summary>
    /// <value>The name of the aggregate.</value>
    string DomainName { get; }

    /// <summary>
    /// Creates this instance.
    /// </summary>
    /// <returns>IDomainAggregate.</returns>
    IDomainAggregate Create();
}