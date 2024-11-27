// <copyright file="MemoryIdDescriptionService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UI.Components.Services;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.UI.Components.ViewModels;

/// <summary>
/// Represents a memory based implementation of the <see cref="Hexalith.UI.Components.Services.IIdDescriptionService" />.
/// </summary>
public class MemoryIdDescriptionService : IIdDescriptionService
{
    private readonly IQueryable<IdDescription> _query;

    /// <summary>
    /// Initializes a new instance of the <see cref="MemoryIdDescriptionService"/> class.
    /// </summary>
    /// <param name="data">The data to be used for the service.</param>
    public MemoryIdDescriptionService(IEnumerable<IdDescription> data) => _query = data.AsQueryable();

    /// <summary>
    /// Initializes a new instance of the <see cref="MemoryIdDescriptionService"/> class.
    /// </summary>
    /// <param name="query">The query to be used for the service.</param>
    public MemoryIdDescriptionService(IQueryable<IdDescription> query) => _query = query;

    /// <inheritdoc/>
    public Task<IdDescription> GetIdDescriptionAsync(string id, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return Task.FromException<IdDescription>(new ArgumentNullException(nameof(id)));
        }

        IdDescription? result = _query.SingleOrDefault(d => d.Id == id);
        return result is null
            ? Task.FromException<IdDescription>(new KeyNotFoundException($"Id '{id}' not found."))
            : Task.FromResult(result);
    }

    /// <inheritdoc/>
    public Task<IEnumerable<IdDescription>> GetIdDescriptionsAsync(int skip, int count, CancellationToken cancellationToken)
    {
        IQueryable<IdDescription> result = _query.OrderBy(p => p.Description);
        if (skip > 0)
        {
            result = result.Skip(skip);
        }

        if (count > 0)
        {
            result = result.Take(count);
        }

        return Task.FromResult<IEnumerable<IdDescription>>([.. result]);
    }

    /// <inheritdoc/>
    public Task<IEnumerable<IdDescription>> SearchIdDescriptionsAsync(string searchText, int skip, int count, CancellationToken cancellationToken)
    {
        IQueryable<IdDescription> result = _query.OrderBy(p => p.Description);
        if (!string.IsNullOrWhiteSpace(searchText))
        {
            string[] tokens = searchText.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            CompareInfo compareInfo = CultureInfo.InvariantCulture.CompareInfo;

            result = result.Where(d => tokens.All(token =>
                compareInfo.IndexOf(
                    d.Description,
                    token,
                    CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0) ||
                    d.Id.Contains(searchText));
        }

        if (skip > 0)
        {
            result = result.Skip(skip);
        }

        if (count > 0)
        {
            result = result.Take(count);
        }

        return Task.FromResult<IEnumerable<IdDescription>>(result);
    }
}