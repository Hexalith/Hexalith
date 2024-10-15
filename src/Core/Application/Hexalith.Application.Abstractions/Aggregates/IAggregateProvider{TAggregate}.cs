// <copyright file="IAggregateProvider{TAggregate}.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Aggregates;

using Hexalith.Domain.Aggregates;

/// <summary>
/// Interface IAggregateProvider.
/// </summary>
/// <typeparam name="TAggregate">The type of the t aggregate.</typeparam>
[Obsolete("This interface is not used anymore. Use IDomainAggregateProvider instead.", true)]
public interface IAggregateProvider<TAggregate> : IAggregateProvider
    where TAggregate : IDomainAggregate, new()
{
    /// <summary>
    /// Creates this instance.
    /// </summary>
    /// <returns>TAggregate.</returns>
    new TAggregate Create();
}