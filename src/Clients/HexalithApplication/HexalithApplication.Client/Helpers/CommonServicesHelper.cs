// <copyright file="CommonServicesHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace HexalithApplication.Client.Helpers;

using Hexalith.Infrastructure.GoogleMaps.Helpers;

/// <summary>
/// Helper class for adding common services to the service collection.
/// </summary>
public static class CommonServicesHelper
{
    /// <summary>
    /// Adds common services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddCommonServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddMemoryCache()
            .AddGooglePlacesServices(configuration);
    }
}
