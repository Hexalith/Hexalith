// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.GoogleMaps.Abstractions
// Author           : Jérôme Piquot
// Created          : 01-19-2024
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-21-2024
// ***********************************************************************
// <copyright file="IGooglePlaceService.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.GoogleMaps.Services;

using Hexalith.Domain.ValueObjets;
using Hexalith.Infrastructure.GoogleMaps.Models;

/// <summary>
/// Interface IGooglePlaceService
/// Extends the <see cref="IAsyncDisposable" />.
/// </summary>
/// <seealso cref="IAsyncDisposable" />
public interface IGooglePlaceService
{
    /// <summary>
    /// Gets the autocomplete options asynchronous.
    /// </summary>
    /// <param name="search">The search.</param>
    /// <param name="cultureCode">The culture code.</param>
    /// <param name="maxResultCount">The maximum result count.</param>
    /// <param name="latitude">The latitude.</param>
    /// <param name="longitude">The longitude.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;IEnumerable&lt;GooglePlace&gt;&gt;.</returns>
    Task<IEnumerable<GooglePlace>> GetAutocompleteOptionsAsync(
        string search,
        string cultureCode,
        int maxResultCount,
        double? latitude,
        double? longitude,
        CancellationToken cancellationToken);

    /// <summary>
    /// Gets the autocomplete text asynchronous.
    /// </summary>
    /// <param name="search">The search.</param>
    /// <param name="cultureCode">The culture code.</param>
    /// <param name="maxResultCount">The maximum result count.</param>
    /// <param name="latitude">The latitude.</param>
    /// <param name="longitude">The longitude.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.String&gt;.</returns>
    Task<string> GetAutocompleteTextAsync(
        string search,
        string cultureCode,
        int maxResultCount,
        double? latitude,
        double? longitude,
        CancellationToken cancellationToken);

    /// <summary>
    /// Gets the postal address asynchronous.
    /// </summary>
    /// <param name="placeId">The place identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;PostalAddress&gt;.</returns>
    Task<PostalAddress> GetPostalAddressAsync(string placeId, CancellationToken cancellationToken);
}