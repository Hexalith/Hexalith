// <copyright file="AggregateProjectionUpdateEventHandler{TEvent,TState}.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Projections;

using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.MessageMetadatas;
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
    where TEvent : class
    where TState : class, IDomainAggregate, new()
{
    /// <inheritdoc/>
    [Obsolete]
    public override async Task ApplyAsync(TEvent baseEvent, Metadata metadata, CancellationToken cancellationToken)
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
            aggregate = await GetProjectionAsync(metadata.Message.Aggregate.Id, cancellationToken)
                .ConfigureAwait(false) ?? new TState();
        }

        ApplyResult result = aggregate.Apply(baseEvent);
        await SaveProjectionAsync(metadata.Message.Aggregate.Id, (TState)result.Aggregate, cancellationToken).ConfigureAwait(false);
    }
}