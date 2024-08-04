// <copyright file="MemoryIdDescriptionService.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UI.Components.Services;

using System;
using System.Collections.Generic;
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
        => Task.FromResult(_query.Single(d => d.Id == id));

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
        IQueryable<IdDescription> result = _query
            .Where(p =>
                p.Description.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                p.Id.Contains(searchText, StringComparison.OrdinalIgnoreCase))
            .OrderBy(p => p.Description);
        if (skip > 0)
        {
            result = result.Skip(skip);
        }

        if (count > 0)
        {
            result = result.Take(count);
        }

        List<IdDescription> list = result.ToList();
        return Task.FromResult<IEnumerable<IdDescription>>(list);
    }
}