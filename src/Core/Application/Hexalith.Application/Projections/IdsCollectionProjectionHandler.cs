// <copyright file="IdsCollectionProjectionHandler.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Projections;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Metadatas;
using Hexalith.Extensions.Helpers;

/// <summary>
/// Handler for updating projections of ID collections based on events.
/// </summary>
/// <typeparam name="TEvent">The type of the event.</typeparam>
public abstract partial class IdsCollectionProjectionHandler<TEvent>(
    IProjectionFactory<IdCollection> factory)
    : IProjectionUpdateHandler<TEvent>
    where TEvent : class
{
    /// <summary>
    /// Gets or sets the page size for the ID collection.
    /// </summary>
    public int PageSize { get; set; } = 1000;

    /// <inheritdoc/>
    public virtual async Task ApplyAsync(TEvent baseEvent, Metadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        ArgumentNullException.ThrowIfNull(metadata);
        IdCollection? currentValue = null;
        string pageId = metadata.Message.Aggregate.Name;
        string? freeSpacePageId = null;
        bool remove = IsRemoveEvent(baseEvent);

        // Loop through the pages to find a page with the current file type ID.
        do
        {
            currentValue = await factory
            .GetStateAsync(pageId, cancellationToken)
            .ConfigureAwait(false)
            ?? new IdCollection(null, []);

            if (currentValue.Ids.Any(p => p == metadata.AggregateGlobalId))
            {
                // The file type ID exists in the collection. Remove it if needed.
                if (remove)
                {
                    await factory
                        .SetStateAsync(
                            pageId,
                            currentValue with { Ids = currentValue.Ids.Where(p => p != metadata.AggregateGlobalId) },
                            cancellationToken)
                        .ConfigureAwait(false);
                }

                return;
            }

            if (currentValue.NextPageId is not null)
            {
                pageId = metadata.Message.Aggregate.Name + currentValue.NextPageId.Value.ToInvariantString();
            }

            if (currentValue.Ids.Count() < PageSize)
            {
                freeSpacePageId = pageId;
            }
        }
        while (currentValue.NextPageId is not null);
        if (remove)
        {
            // The file type ID does not exist in the collection. Nothing to do.
            return;
        }

        // The file type ID is not in the collection. Add it.
        if (freeSpacePageId is null)
        {
            // The collection is full. Create a new page.
            int newPageId = currentValue.NextPageId is null ? 1 : currentValue.NextPageId.Value + 1;

            // Update the current page next page identifier to the new page.
            await factory
                .SetStateAsync(
                pageId,
                currentValue with { NextPageId = newPageId },
                cancellationToken)
                .ConfigureAwait(false);
            pageId = metadata.Message.Aggregate.Name + newPageId.ToInvariantString();
            await factory
                .SetStateAsync(
                pageId,
                new IdCollection(null, [metadata.AggregateGlobalId]),
                cancellationToken)
                .ConfigureAwait(false);
        }
        else
        {
            await factory
            .SetStateAsync(
                pageId,
                currentValue with { Ids = currentValue.Ids.Append(metadata.AggregateGlobalId).Distinct().OrderBy(p => p) },
                cancellationToken)
            .ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Determines whether the specified event is a remove event.
    /// </summary>
    /// <param name="baseEvent">The event to check.</param>
    /// <returns><c>true</c> if the specified event is a remove event; otherwise, <c>false</c>.</returns>
    protected abstract bool IsRemoveEvent(TEvent baseEvent);
}