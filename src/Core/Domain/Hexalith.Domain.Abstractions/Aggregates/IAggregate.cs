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

using System.Collections.Generic;

using Hexalith.Domain.Abstractions.Events;

/// <summary>
/// Interface IAggregate.
/// </summary>
public interface IAggregate
{
    /// <summary>
    /// Applies the specified events.
    /// </summary>
    /// <param name="events">The events.</param>
    /// <returns>IAggregate.</returns>
    static abstract IAggregate Apply(IEnumerable<BaseEvent> events);

    /// <summary>
    /// Applies the specified domain event.
    /// </summary>
    /// <param name="domainEvent">The domain event.</param>
    /// <returns>IAggregate.</returns>
    IAggregate Apply(BaseEvent domainEvent);
}