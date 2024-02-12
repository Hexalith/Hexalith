// <copyright file="PostalAddressUIHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UI.PostalAddresses.Helpers;

using Hexalith.Infrastructure.GoogleMaps.Helpers;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Helper class for managing postal addresses in the UI.
/// </summary>
/// <remarks>
/// This class provides extension methods to IServiceCollection for adding postal address UI services.
/// </remarks>
public static class PostalAddressUIHelper
{
    /// <summary>
    /// Adds postal address UI services to the specified IServiceCollection.
    /// </summary>
    /// <param name="services">The IServiceCollection to add the services to.</param>
    /// <param name="configuration">The IConfiguration instance.</param>
    /// <returns>The modified IServiceCollection.</returns>
    public static IServiceCollection AddPostalAddressUI(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddGeolocationServices()
            .AddGooglePlacesServices(configuration);
    }
}