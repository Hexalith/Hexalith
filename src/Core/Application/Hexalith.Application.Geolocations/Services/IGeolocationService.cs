// <copyright file="IGeolocationService.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Geolocations.Services;

using Hexalith.Domain.ValueObjets;

/// <summary>
/// Represents a service for retrieving the current geolocation.
/// </summary>
public interface IGeolocationService
{
    /// <summary>
    /// Retrieves the current geolocation asynchronously.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The current geolocation, or null if the geolocation cannot be determined.</returns>
    Task<GeoLocation?> GetCurrentLocationAsync(CancellationToken cancellationToken);
}