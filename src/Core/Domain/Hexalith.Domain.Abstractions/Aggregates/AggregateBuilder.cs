// ***********************************************************************
// Assembly         : Hexalith.Domain.Abstractions
// Author           : Jérôme Piquot
// Created          : 04-26-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 04-26-2023
// ***********************************************************************
// <copyright file="AggregateBuilder.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.Abstractions.Aggregates;

using System;
using System.Collections.Generic;
using System.Linq;

using Hexalith.Domain.Abstractions.Events;

/// <summary>
/// Class AggregateBuilder.
/// </summary>
public class AggregateBuilder
{
    /// <summary>
    /// The events.
    /// </summary>
    private BaseEvent[]? _events;

    /// <summary>
    /// The initializer.
    /// </summary>
    private Func<BaseEvent, IAggregate>? _initializer;

    /// <summary>
    /// Builds this instance.
    /// </summary>
    /// <returns>IAggregate.</returns>
    /// <exception cref="System.InvalidOperationException">Events are not set.</exception>
    /// <exception cref="System.InvalidOperationException">Initializer is not set.</exception>
    public IAggregate Build()
    {
        if (_events is null || _events.Length == 0)
        {
            throw new InvalidOperationException("The event list is null or empty.");
        }

        if (_initializer is null)
        {
            throw new InvalidOperationException("Initializer is not set.");
        }

        IAggregate aggregate = _initializer(_events[0]);
        foreach (BaseEvent @event in _events[1..])
        {
            aggregate = aggregate.Apply(@event);
        }

        return aggregate;
    }

    /// <summary>
    /// Events the specified events.
    /// </summary>
    /// <param name="events">The events.</param>
    /// <returns>AggregateBuilder.</returns>
    /// <exception cref="System.ArgumentNullException">Null.</exception>
    public AggregateBuilder Events(IEnumerable<BaseEvent> events)
    {
        ArgumentNullException.ThrowIfNull(events);
        _events = events.ToArray();
        return this;
    }

    /// <summary>
    /// Initializers the specified initializer.
    /// </summary>
    /// <param name="initializer">The initializer.</param>
    /// <returns>AggregateBuilder.</returns>
    /// <exception cref="System.ArgumentNullException">Null.</exception>
    public AggregateBuilder Initializer(Func<BaseEvent, IAggregate> initializer)
    {
        ArgumentNullException.ThrowIfNull(initializer);
        _initializer = initializer;
        return this;
    }
}