// <copyright file="SequentialStringListActor.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Actors;

using System;
using System.Collections.Generic;

using Dapr.Actors.Runtime;

using Hexalith.Extensions.Helpers;

/// <summary>
/// Represents an actor that manages a sequential list of strings.
/// </summary>
public class SequentialStringListActor : Actor, ISequentialStringListActor
{
    private const int _pageSize = 1024;
    private SequentialStringListPage? _state;

    /// <summary>
    /// Initializes a new instance of the <see cref="SequentialStringListActor"/> class.
    /// </summary>
    /// <param name="host">The actor host.</param>
    /// <param name="stateManager">The actor state manager.</param>
    public SequentialStringListActor(ActorHost host, IActorStateManager? stateManager = null)
        : base(host)
    {
        ArgumentNullException.ThrowIfNull(host);
        if (stateManager is not null)
        {
            // Set the state manager if it is not null. This is useful for testing.
            StateManager = stateManager;
        }
    }

    /// <inheritdoc/>
    public async Task AddAsync(string value)
    {
        (SequentialStringListPage? page, int? freeSpacePageNumber) = await FindPageAsync(value);
        if (page != null)
        {
            // Since value already exists, we do not need to add it.
            return;
        }

        if (freeSpacePageNumber is null)
        {
            throw new InvalidOperationException("Could not add value: Last page number is null");
        }

        _state = await GetPageAsync(freeSpacePageNumber.Value);

        // If the state is null, we have reached the end of the list and need to create a new page.
        _state = _state is null
            ? new SequentialStringListPage(freeSpacePageNumber.Value, [value])
            : _state with { Data = [.. _state.Data, value] };
        await SaveAsync();
    }

    /// <inheritdoc/>
    public async Task<bool> ExistsAsync(string value)
        => (await FindPageAsync(value)).Page is not null;

    /// <inheritdoc/>
    public async Task<IEnumerable<string>?> ReadAsync(int skip, int take)
    {
        // If take is equal or under 0, return all items
        _ = take < 0 ? 0 : take;

        // If skip is equal or under 0, start from the beginning
        _ = skip < 0 ? 0 : skip;

        // Ignore the pages that are not needed
        int page = 0;
        List<string> result = [];
        int count = 0;
        while ((_state = await GetPageAsync(page++)) is not null)
        {
            int pageItemCount = _state.Data.Count();
            if (skip > count + pageItemCount)
            {
                // Skip the page if the count is less than the skip count
                continue;
            }

            IEnumerable<string> segment = _state.Data;
            if (skip < count)
            {
                // Skip the items that are not needed in the page
                segment = segment.Skip(skip - count);
            }

            if (take > 0 && result.Count + segment.Count() > take)
            {
                // If the result count is greater than the take count, take the remaining items
                segment = segment.Take(take - result.Count);
            }

            result.AddRange(segment);
        }

        return result;
    }

    /// <inheritdoc/>
    public async Task RemoveAsync(string value)
    {
        (SequentialStringListPage? page, int? _) = await FindPageAsync(value);
        if (page is null)
        {
            // Since value does not exist in the list, we do not need to remove it.
            return;
        }

        _state = page with { Data = [.. page.Data.Where(p => p != value)] };
        await SaveAsync();
    }

    private async Task<(SequentialStringListPage? Page, int? FreeSpacePageNumber)> FindPageAsync(string value)
    {
        int pageId = 0;
        int? freeSpacePageNumber = null;
        do
        {
            _state = await GetPageAsync(pageId);
            if (_state is null)
            {
                // If the page is null, we have reached the end of the list. If all pages are full, return the new page number.
                return (null, freeSpacePageNumber ?? pageId);
            }

            if (_state.Data.Contains(value))
            {
                // Since the value already exists, we do not need to return the free space page number as it will not be added.
                return (_state, null);
            }

            if (freeSpacePageNumber is null && _state.Data.Count() < _pageSize)
            {
                // The first free space page number is the first page with less than the maximum number of elements.
                freeSpacePageNumber = pageId;
            }

            pageId++;
        }
        while (true);
    }

    private async Task<SequentialStringListPage?> GetPageAsync(int pageNumber)
    {
        if (_state is null || _state.PageNumber != pageNumber)
        {
            ConditionalValue<IEnumerable<string>> result = await StateManager
                .TryGetStateAsync<IEnumerable<string>>(pageNumber.ToInvariantString(), CancellationToken.None);
            if (!result.HasValue)
            {
                return null;
            }

            _state = new SequentialStringListPage(pageNumber, result.Value);
        }

        return _state;
    }

    private async Task SaveAsync()
    {
        if (_state is null)
        {
            throw new InvalidOperationException("Could not save state: State is null");
        }

        await StateManager.SetStateAsync(_state.PageNumber.ToInvariantString(), _state.Data, CancellationToken.None);
        await StateManager.SaveStateAsync();
    }
}