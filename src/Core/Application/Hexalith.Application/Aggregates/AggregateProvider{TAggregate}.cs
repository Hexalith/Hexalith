// ***********************************************************************
// Assembly         : Hexalith.Application
// Author           : Jérôme Piquot
// Created          : 01-10-2024
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-10-2024
// ***********************************************************************
// <copyright file="AggregateProvider{TAggregate}.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Aggregates;

using System;

using Hexalith.Domain.Aggregates;

/// <summary>
/// Class AggregateProvider.
/// Implements the <see cref="Hexalith.Application.Aggregates.IAggregateProvider{TAggregate}" />.
/// </summary>
/// <typeparam name="TAggregate">The type of the t aggregate.</typeparam>
/// <seealso cref="Hexalith.Application.Aggregates.IAggregateProvider{TAggregate}" />
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
    IAggregate IAggregateProvider.Create() => Create();
}