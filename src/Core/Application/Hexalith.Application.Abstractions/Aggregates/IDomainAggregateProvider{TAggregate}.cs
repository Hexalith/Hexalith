// <copyright file="IDomainAggregateProvider{TAggregate}.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Aggregates;

using Hexalith.Domain.Aggregates;

/// <summary>
/// Interface IDomainAggregateProvider.
/// </summary>
/// <typeparam name="TAggregate">The type of the t aggregate.</typeparam>
public interface IDomainAggregateProvider<out TAggregate> : IDomainAggregateProvider
    where TAggregate : IDomainAggregate, new()
{
    /// <summary>
    /// Creates this instance.
    /// </summary>
    /// <returns>TAggregate.</returns>
    new TAggregate Create();
}