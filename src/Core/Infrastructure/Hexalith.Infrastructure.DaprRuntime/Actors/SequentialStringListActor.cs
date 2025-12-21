// <copyright file="SequentialStringListActor.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Actors;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Dapr.Actors.Runtime;

using Hexalith.Commons.Strings;
using Hexalith.Extensions.Helpers;

/// <summary>
/// Represents an actor that manages a sequential list of strings.
/// It uses a cached _state field to minimize database reads for repeated operations.
/// </summary>
public class SequentialStringListActor : Actor, ISequentialStringListActor
{
    private const int _pageSize = 1024;

    // Cached in-memory state of the currently loaded page. If most instances are a single page, this reduces overhead.
    private SequentialStringListPage? _state;

    /// <summary>
    /// Initializes a new instance of the <see cref="SequentialStringListActor"/> class.
    /// </summary>
    /// <param name="host">The actor host.</param>
    /// <param name="stateManager">The actor state manager (for testing or customization).</param>
    public SequentialStringListActor(ActorHost host, IActorStateManager? stateManager = null)
        : base(host)
    {
        ArgumentNullException.ThrowIfNull(host);

        // If a custom state manager is provided (e.g., in unit tests), use it.
        if (stateManager is not null)
        {
            StateManager = stateManager;
        }
    }

    /// <inheritdoc/>
    public async Task AddAsync(string value)
    {
        (SequentialStringListPage? existingPage, int? freeSpacePageNumber) = await FindPageAsync(value).ConfigureAwait(false);

        // If the value already exists, do nothing.
        if (existingPage is not null)
        {
            return;
        }

        // If we can't determine where to place the new value, throw.
        if (freeSpacePageNumber is null)
        {
            throw new InvalidOperationException("Could not add value: No valid page number found.");
        }

        SequentialStringListPage? pageToUpdate = await GetPageAsync(freeSpacePageNumber.Value).ConfigureAwait(false);

        SequentialStringListPage newPage;
        if (pageToUpdate is null)
        {
            // Create a new page if it doesn't exist
            newPage = new SequentialStringListPage(freeSpacePageNumber.Value, [value]);
        }
        else
        {
            // Append the new value to the existing page
            List<string> newData = [.. pageToUpdate.Data];
            newData.Add(value);
            newPage = pageToUpdate with { Data = newData };
        }

        await SavePageAsync(newPage).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<bool> ExistsAsync(string value)
    {
        (SequentialStringListPage? page, int? _) = await FindPageAsync(value).ConfigureAwait(false);
        return page is not null;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<string>> ReadAsync(int skip, int take)
    {
        // Normalize skip and take
        if (skip < 0)
        {
            skip = 0;
        }

        // If take <= 0 means user wants all items
        if (take <= 0)
        {
            take = int.MaxValue;
        }

        List<string> result = [];
        int countSoFar = 0;
        int pageIndex = 0;

        SequentialStringListPage? currentPage;
        while ((currentPage = await GetPageAsync(pageIndex++).ConfigureAwait(false)) is not null)
        {
            int pageItemCount = currentPage.Data.Count();

            // If the entire page is before our skip window, skip it entirely
            if (skip >= countSoFar + pageItemCount)
            {
                countSoFar += pageItemCount;
                continue;
            }

            // Determine how many items to skip in the current page
            int itemsToSkipInPage = Math.Max(0, skip - countSoFar);

            // Skip the required items from this page
            IEnumerable<string> segment = currentPage.Data.Skip(itemsToSkipInPage);

            // Update the count after skipping
            countSoFar += itemsToSkipInPage;

            // Determine how many items we can still take
            int remaining = take - result.Count;
            if (remaining <= 0)
            {
                break;
            }

            // Take only the required remaining items from this page
            List<string> pageSegment = [.. segment.Take(remaining)];
            result.AddRange(pageSegment);

            // Update the count after taking items
            countSoFar += pageItemCount - itemsToSkipInPage;

            // If we've reached the required number of items, stop
            if (result.Count >= take)
            {
                break;
            }
        }

        return result;
    }

    /// <inheritdoc/>
    public async Task RemoveAsync(string value)
    {
        (SequentialStringListPage? page, int? _) = await FindPageAsync(value).ConfigureAwait(false);

        if (page is null)
        {
            // Value does not exist, nothing to remove.
            return;
        }

        List<string> newData = [.. page.Data.Where(p => p != value)];
        SequentialStringListPage newPage = page with { Data = newData };

        await SavePageAsync(newPage).ConfigureAwait(false);
    }

    /// <summary>
    /// Finds the page containing the specified value, or identifies where it can be inserted.
    /// </summary>
    private async Task<(SequentialStringListPage? Page, int? FreeSpacePageNumber)> FindPageAsync(string value)
    {
        int pageId = 0;
        int? freeSpacePageNumber = null;

        while (true)
        {
            SequentialStringListPage? currentPage = await GetPageAsync(pageId).ConfigureAwait(false);
            if (currentPage is null)
            {
                // Reached the end of existing pages.
                // If we didn't find a free space page earlier, we can add at this new page index.
                return (null, freeSpacePageNumber ?? pageId);
            }

            // Check if current page has the value
            if (currentPage.Data.Contains(value))
            {
                return (currentPage, null);
            }

            // If we haven't recorded a free space page yet and this page is not full
            if (freeSpacePageNumber is null && currentPage.Data.Count() < _pageSize)
            {
                freeSpacePageNumber = pageId;
            }

            pageId++;
        }
    }

    /// <summary>
    /// Retrieves the specified page from state, using the cached _state if possible.
    /// </summary>
    private async Task<SequentialStringListPage?> GetPageAsync(int pageNumber)
    {
        // Check if the requested page is already cached
        if (_state is not null && _state.PageNumber == pageNumber)
        {
            return _state;
        }

        ConditionalValue<IEnumerable<string>> result = await StateManager
            .TryGetStateAsync<IEnumerable<string>>(pageNumber.ToInvariantString(), CancellationToken.None)
            .ConfigureAwait(false);

        if (!result.HasValue)
        {
            // Page does not exist in state
            _state = null;
            return null;
        }

        // Convert IEnumerable<string> to a List<string> for easier manipulation
        List<string> dataList = result.Value as List<string> ?? [.. result.Value];
        _state = new SequentialStringListPage(pageNumber, dataList);
        return _state;
    }

    /// <summary>
    /// Saves the page to the state manager and updates the cached state.
    /// </summary>
    private async Task SavePageAsync(SequentialStringListPage page)
    {
        _state = page;
        await StateManager.SetStateAsync(page.PageNumber.ToInvariantString(), page.Data, CancellationToken.None)
            .ConfigureAwait(false);
        await StateManager.SaveStateAsync().ConfigureAwait(false);
    }
}