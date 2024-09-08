// <copyright file="AggregateProvider{TAggregate}.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.Application.Aggregates;

using System;

using Hexalith.Domain.Aggregates;

/// <summary>
/// Class AggregateProvider.
/// Implements the <see cref="Hexalith.Application.Aggregates.IAggregateProvider{TAggregate}" />.
/// </summary>
/// <typeparam name="TAggregate">The type of the t aggregate.</typeparam>
/// <seealso cref="Hexalith.Application.Aggregates.IAggregateProvider{TAggregate}" />
[Obsolete]
public class AggregateProvider<TAggregate> : IAggregateProvider<TAggregate>
    where TAggregate : IAggregate, new()
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
    [Obsolete]
    IAggregate IAggregateProvider.Create() => Create();
}