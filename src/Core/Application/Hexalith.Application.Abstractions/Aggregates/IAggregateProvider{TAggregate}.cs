// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 01-10-2024
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-10-2024
// ***********************************************************************
// <copyright file="IAggregateProvider{TAggregate}.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

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