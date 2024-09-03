// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprRuntime
// Author           : Jérôme Piquot
// Created          : 12-19-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 12-19-2023
// ***********************************************************************
// <copyright file="AggregateProjectionUpdateEventHandler{TEvent,TState}.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.DaprRuntime.Projections;

using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Metadatas;
using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Events;

#pragma warning disable CA1711 // Identifiers should not have incorrect suffix

/// <summary>
/// Represents the base class for aggregate projection update event handlers.
/// </summary>
/// <typeparam name="TEvent">The type of the event.</typeparam>
/// <typeparam name="TState">The type of the aggregate state.</typeparam>
/// <remarks>
/// Initializes a new instance of the <see cref="AggregateProjectionUpdateEventHandler{TEvent, TState}"/> class.
/// </remarks>
/// <param name="factory">state factory.</param>
public class AggregateProjectionUpdateEventHandler<TEvent, TState>(
    IActorProjectionFactory<TState> factory) :
    KeyValueActorProjectionUpdateEventHandlerBase<TEvent, TState>(factory)
    where TEvent : IEvent
    where TState : class, IAggregate, new()
{
    /// <inheritdoc/>
    public override async Task ApplyAsync(TEvent baseEvent, IMetadata metadata, CancellationToken cancellationToken)
    {
        TState? aggregate;

        // If the event is a snapshot, we initialize a new aggregate from the snapshot
        if (baseEvent is SnapshotEvent s)
        {
            aggregate = new TState();

            // If the event is a snapshot, we must check the aggregate name
            if (s.AggregateName != aggregate.AggregateName)
            {
                // If the aggregate name is not the same, we ignore the event
                return;
            }
        }
        else
        {
            // If the event is not a snapshot, we load the aggregate from the store
            aggregate = await GetProjectionAsync(baseEvent.AggregateId, cancellationToken)
                .ConfigureAwait(false) ?? new TState();
        }

        (IAggregate newAggregate, _) = aggregate.Apply((IEnumerable<BaseEvent>)baseEvent);
        await SaveProjectionAsync(baseEvent.AggregateId, (TState)newAggregate, cancellationToken).ConfigureAwait(false);
    }
}