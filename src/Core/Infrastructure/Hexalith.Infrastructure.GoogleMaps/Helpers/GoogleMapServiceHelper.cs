// ***********************************************************************
// Assembly         : HexalithApplication
// Author           : Jérôme Piquot
// Created          : 01-19-2024
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-19-2024
// ***********************************************************************
// <copyright file="GoogleMapServiceHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.GoogleMaps.Helpers;

using GoogleApi.Extensions;

using Hexalith.Application.Geolocations.Services;
using Hexalith.Extensions.Configuration;
using Hexalith.Infrastructure.GoogleMaps.Abstractions.Configurations;
using Hexalith.Infrastructure.GoogleMaps.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Class GoogleMapServiceHelper.
/// </summary>
public static class GoogleMapServiceHelper
{
    /// <summary>
    /// Adds the google places services.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>IServiceCollection.</returns>
    public static IServiceCollection AddGooglePlacesServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .ConfigureSettings<GoogleSettings>(configuration)
            .AddTransient<IPlaceService, GooglePlaceService>()
            .AddGoogleApiClients();
    }
}