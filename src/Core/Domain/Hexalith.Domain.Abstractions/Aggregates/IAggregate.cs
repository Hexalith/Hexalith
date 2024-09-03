// ***********************************************************************
// Assembly         : Hexalith.Domain.Abstractions
// Author           : Jérôme Piquot
// Created          : 04-25-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 04-25-2023
// ***********************************************************************
// <copyright file="IAggregate.cs">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.Aggregates;

using System.Diagnostics.CodeAnalysis;

using Hexalith.Domain.Events;
using Hexalith.Domain.Messages;

/// <summary>
/// Interface IAggregate.
/// </summary
[Obsolete("This interface is obsolete. Use Hexalith.Domain.Abstractions.Aggregates.IDomainAggregate instead.", false)]
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
    (IAggregate Aggregate, IEnumerable<BaseMessage> Messages) Apply(BaseEvent domainEvent);

    /// <summary>
    /// Applies the specified domain event.
    /// </summary>
    /// <param name="events">The domain events.</param>
    /// <returns>IAggregate.</returns>
    (IAggregate Aggregate, IEnumerable<BaseMessage> Messages) Apply([NotNull] IEnumerable<BaseEvent> events)
    {
        ArgumentNullException.ThrowIfNull(events);
        IAggregate aggregate = this;
        List<BaseMessage> newMessages = [];
        foreach (BaseEvent e in events)
        {
            if (e.AggregateName != aggregate.AggregateName || (IsInitialized() && e.AggregateId != aggregate.AggregateId))
            {
                string aggregateId = IsInitialized() ? $"/{e.AggregateId}" : string.Empty;
                throw new InvalidOperationException(
                    $"The event '{e.TypeName}' can only be applied to aggregate '{e.AggregateName}' with Id {e.AggregateId}. " +
                    $"It cannot be applied to aggregate {aggregate.AggregateName}{aggregateId}.");
            }

            (aggregate, IEnumerable<BaseMessage>? messages) = aggregate.Apply(e);
            newMessages.AddRange(messages);
        }

        return (aggregate, newMessages);
    }

    /// <summary>
    /// Determines whether this instance is initialized.
    /// </summary>
    /// <returns><c>true</c> if this instance is initialized; otherwise, <c>false</c>.</returns>
    bool IsInitialized();
}