// <copyright file="IDomainAggregate.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Aggregates;

/// <summary>
/// Interface IDomainAggregate.
/// </summary>
public interface IDomainAggregate
{
    /// <summary>
    /// Gets the aggregate identifier.
    /// </summary>
    /// <value>The aggregate identifier.</value>
    string AggregateId { get; }

    /// <summary>
    /// Gets the name of the aggregate.
    /// </summary>
    /// <value>The name of the aggregate.</value>
    string AggregateName { get; }

    /// <summary>
    /// Applies the specified domain event.
    /// </summary>
    /// <param name="domainEvent">The domain event.</param>
    /// <returns>IDomainAggregate.</returns>
    ApplyResult Apply(object domainEvent);

    /// <summary>
    /// Determines whether this instance is initialized.
    /// </summary>
    /// <returns><c>true</c> if this instance is initialized; otherwise, <c>false</c>.</returns>
    bool IsInitialized() => !string.IsNullOrWhiteSpace(AggregateId);
}