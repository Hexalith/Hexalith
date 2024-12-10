// <copyright file="IdsCollectionProjectionHandler{TEvent}.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Requests;

using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Metadatas;
using Hexalith.Application.Projections;
using Hexalith.Application.Services;

/// <summary>
/// Handles projection updates for a collection of IDs.
/// </summary>
/// <typeparam name="TEvent">The type of the event.</typeparam>
/// <remarks>
/// Initializes a new instance of the <see cref="IdsCollectionProjectionHandler{TEvent}"/> class.
/// </remarks>
/// <param name="factory">The factory to create ID collection services.</param>
public class IdsCollectionProjectionHandler<TEvent>(IIdCollectionFactory factory) : IProjectionUpdateHandler<TEvent>
    where TEvent : class
{
    /// <inheritdoc/>
    public async Task ApplyAsync(TEvent baseEvent, Metadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        ArgumentNullException.ThrowIfNull(metadata);
        IIdCollectionService service = factory.CreateService(
            IIdCollectionFactory.GetAggregateCollectionName(metadata.Message.Aggregate.Name),
            metadata.Context.PartitionId);
        await service.AddAsync(metadata.AggregateGlobalId, cancellationToken).ConfigureAwait(false);
    }
}