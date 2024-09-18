// <copyright file="DomainAggregateProvider{TAggregate}.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.Application.Aggregates;

using System;

using Hexalith.Domain.Aggregates;

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