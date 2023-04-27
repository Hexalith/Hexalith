// ***********************************************************************
// Assembly         : Hexalith.Domain.Abstractions
// Author           : Jérôme Piquot
// Created          : 04-25-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 04-25-2023
// ***********************************************************************
// <copyright file="IAggregate.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.Abstractions.Aggregates;

using Hexalith.Domain.Abstractions.Events;

/// <summary>
/// Interface IAggregate.
/// </summary>
public interface IAggregate
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
    /// <returns>IAggregate.</returns>
    IAggregate Apply(BaseEvent domainEvent);

    /// <summary>
    /// Applies the specified domain event.
    /// </summary>
    /// <param name="events">The domain events.</param>
    /// <returns>IAggregate.</returns>
    IAggregate Apply(IEnumerable<BaseEvent> events)
    {
        IAggregate aggregate = this;
        foreach (BaseEvent e in events)
        {
            aggregate = aggregate.Apply(e);
        }

        return aggregate;
    }
}