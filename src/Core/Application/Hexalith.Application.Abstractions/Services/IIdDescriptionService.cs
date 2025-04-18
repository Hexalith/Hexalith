// <copyright file="IIdDescriptionService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Services;

using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

using Hexalith.Domains.ValueObjects;

/// <summary>
/// Represents a service for retrieving identifier and description pairs.
/// </summary>
public interface IIdDescriptionService
{
    /// <summary>
    /// Gets the ID-description pair asynchronously based on the specified ID.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="id">The ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the ID-description pair.</returns>
    Task<IIdDescription> GetIdDescriptionAsync(ClaimsPrincipal user, string id, CancellationToken cancellationToken);

    /// <summary>
    /// Gets all ID-description pairs asynchronously.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collection of ID-description pairs.</returns>
    Task<IEnumerable<IIdDescription>> GetIdDescriptionsAsync(ClaimsPrincipal user, CancellationToken cancellationToken)
        => GetIdDescriptionsAsync(user, 0, 0, cancellationToken);

    /// <summary>
    /// Gets a range of ID-description pairs asynchronously.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="skip">The number of items to skip.</param>
    /// <param name="take">The number of items to retrieve.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collection of ID-description pairs.</returns>
    Task<IEnumerable<IIdDescription>> GetIdDescriptionsAsync(ClaimsPrincipal user, int skip, int take, CancellationToken cancellationToken);

    /// <summary>
    /// Searches for ID-description pairs asynchronously based on the specified search text.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="searchText">The search text.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collection of ID-description pairs.</returns>
    Task<IEnumerable<IIdDescription>> SearchIdDescriptionsAsync(ClaimsPrincipal user, string searchText, CancellationToken cancellationToken)
        => SearchIdDescriptionsAsync(user, searchText, 0, 0, cancellationToken);

    /// <summary>
    /// Searches for ID-description pairs asynchronously based on the specified search text, skip and count.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="searchText">The search text.</param>
    /// <param name="skip">The number of items to skip.</param>
    /// <param name="take">The number of items to retrieve.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collection of ID-description pairs.</returns>
    Task<IEnumerable<IIdDescription>> SearchIdDescriptionsAsync(ClaimsPrincipal user, string searchText, int skip, int take, CancellationToken cancellationToken);
}