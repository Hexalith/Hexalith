// <copyright file="IDomainAggregateProvider{TAggregate}.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.Application.Aggregates;

using Hexalith.Domain.Aggregates;

/// <summary>
/// Interface IAggregateProvider.
/// </summary>
/// <typeparam name="TAggregate">The type of the t aggregate.</typeparam>
public interface IDomainAggregateProvider<TAggregate> : IDomainAggregateProvider
    where TAggregate : IDomainAggregate, new()
{
    /// <summary>
    /// Creates this instance.
    /// </summary>
    /// <returns>TAggregate.</returns>
    new TAggregate Create();
}