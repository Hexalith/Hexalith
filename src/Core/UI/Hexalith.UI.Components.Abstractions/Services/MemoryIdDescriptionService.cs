// <copyright file="MemoryIdDescriptionService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UI.Components.Services;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Services;

/// <summary>
/// Represents a memory based implementation of the <see cref="Application.Services.IIdDescriptionService" />.
/// </summary>
public class MemoryIdDescriptionService : IIdDescriptionService
{
    private readonly IQueryable<IIdDescription> _query;

    /// <summary>
    /// Initializes a new instance of the <see cref="MemoryIdDescriptionService"/> class.
    /// </summary>
    /// <param name="data">The data to be used for the service.</param>
    public MemoryIdDescriptionService(IEnumerable<IIdDescription> data) => _query = data.AsQueryable();

    /// <summary>
    /// Initializes a new instance of the <see cref="MemoryIdDescriptionService"/> class.
    /// </summary>
    /// <param name="query">The query to be used for the service.</param>
    public MemoryIdDescriptionService(IQueryable<IIdDescription> query) => _query = query;

    /// <inheritdoc/>
    public Task<IIdDescription> GetIdDescriptionAsync(ClaimsPrincipal user, string id, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return Task.FromException<IIdDescription>(new ArgumentNullException(nameof(id)));
        }

        IIdDescription? result = _query.SingleOrDefault(d => d.Id == id);
        return result is null
            ? Task.FromException<IIdDescription>(new KeyNotFoundException($"Id '{id}' not found."))
            : Task.FromResult(result);
    }

    /// <inheritdoc/>
    public Task<IEnumerable<IIdDescription>> GetIdDescriptionsAsync(ClaimsPrincipal user, int skip, int count, CancellationToken cancellationToken)
    {
        IQueryable<IIdDescription> result = _query.OrderBy(p => p.Description);
        if (skip > 0)
        {
            result = result.Skip(skip);
        }

        if (count > 0)
        {
            result = result.Take(count);
        }

        return Task.FromResult<IEnumerable<IIdDescription>>([.. result]);
    }

    /// <inheritdoc/>
    public Task<IEnumerable<IIdDescription>> SearchIdDescriptionsAsync(ClaimsPrincipal user, string searchText, int skip, int count, CancellationToken cancellationToken)
    {
        IQueryable<IIdDescription> result = _query.OrderBy(p => p.Description);
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

        return Task.FromResult<IEnumerable<IIdDescription>>(result);
    }
}