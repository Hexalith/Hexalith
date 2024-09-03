// <copyright file="IIdDescriptionService.cs">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UI.Components.Services;

using System.Collections.Generic;
using System.Threading.Tasks;

using Hexalith.UI.Components.ViewModels;

/// <summary>
/// Represents a service for retrieving identifier and description pairs.
/// </summary>
public interface IIdDescriptionService
{
    /// <summary>
    /// Gets the ID-description pair asynchronously based on the specified ID.
    /// </summary>
    /// <param name="id">The ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the ID-description pair.</returns>
    Task<IdDescription> GetIdDescriptionAsync(string id, CancellationToken cancellationToken);

    /// <summary>
    /// Gets all ID-description pairs asynchronously.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collection of ID-description pairs.</returns>
    Task<IEnumerable<IdDescription>> GetIdDescriptionsAsync(CancellationToken cancellationToken)
        => GetIdDescriptionsAsync(0, 0, cancellationToken);

    /// <summary>
    /// Gets a range of ID-description pairs asynchronously.
    /// </summary>
    /// <param name="skip">The number of items to skip.</param>
    /// <param name="count">The number of items to retrieve.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collection of ID-description pairs.</returns>
    Task<IEnumerable<IdDescription>> GetIdDescriptionsAsync(int skip, int count, CancellationToken cancellationToken);

    /// <summary>
    /// Searches for ID-description pairs asynchronously based on the specified search text.
    /// </summary>
    /// <param name="searchText">The search text.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collection of ID-description pairs.</returns>
    Task<IEnumerable<IdDescription>> SearchIdDescriptionsAsync(string searchText, CancellationToken cancellationToken)
        => SearchIdDescriptionsAsync(searchText, 0, 0, cancellationToken);

    /// <summary>
    /// Searches for ID-description pairs asynchronously based on the specified search text, skip and count.
    /// </summary>
    /// <param name="searchText">The search text.</param>
    /// <param name="skip">The number of items to skip.</param>
    /// <param name="count">The number of items to retrieve.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collection of ID-description pairs.</returns>
    Task<IEnumerable<IdDescription>> SearchIdDescriptionsAsync(string searchText, int skip, int count, CancellationToken cancellationToken);
}