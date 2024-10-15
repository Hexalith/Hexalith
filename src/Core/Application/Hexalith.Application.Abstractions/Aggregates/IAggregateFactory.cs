// <copyright file="IAggregateFactory.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Aggregates;

using System;

using Hexalith.Domain.Aggregates;

/// <summary>
/// Interface IAggregateFactory.
/// </summary>
[Obsolete("This interface is obsolete. Use Hexalith.Application.Abstractions.Aggregates.IDomainAggregateFactory instead.", false)]
public interface IAggregateFactory
{
    /// <summary>
    /// Creates the specified aggregate name.
    /// </summary>
    /// <param name="aggregateName">Name of the aggregate.</param>
    /// <returns>IDomainAggregate.</returns>
    IDomainAggregate Create(string aggregateName);

    /// <summary>
    /// Gets the type of the aggregate.
    /// </summary>
    /// <param name="aggregateName">Name of the aggregate.</param>
    /// <returns>Type.</returns>
    Type GetAggregateType(string aggregateName);
}