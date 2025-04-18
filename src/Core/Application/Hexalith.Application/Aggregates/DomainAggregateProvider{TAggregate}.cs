// <copyright file="DomainAggregateProvider{TAggregate}.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Aggregates;

using System;

using Hexalith.Domains;

/// <summary>
/// Class AggregateProvider.
/// Implements the <see cref="Hexalith.Application.Aggregates.IDomainAggregateProvider{TAggregate}" />.
/// </summary>
/// <typeparam name="TAggregate">The type of the t aggregate.</typeparam>
/// <seealso cref="Hexalith.Application.Aggregates.IDomainAggregateProvider{TAggregate}" />
public class DomainAggregateProvider<TAggregate> : IDomainAggregateProvider<TAggregate>
    where TAggregate : IDomainAggregate, new()
{
    /// <summary>
    /// The aggregate name.
    /// </summary>
    private string? _aggregateName;

    /// <inheritdoc/>
    public string AggregateName => _aggregateName ??= Create().AggregateName;

    /// <inheritdoc/>
    public Type AggregateType => typeof(TAggregate);

    /// <inheritdoc/>
    public TAggregate Create() => new();

    /// <inheritdoc/>
    IDomainAggregate IDomainAggregateProvider.Create() => Create();
}