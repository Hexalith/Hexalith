// <copyright file="CommonServicesHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace HexalithApplication.Client.Helpers;

using Hexalith.Infrastructure.GoogleMaps.Helpers;

public static class CommonServicesHelper
{
    public static IServiceCollection AddCommonServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddMemoryCache()
            .AddGooglePlacesServices(configuration);
    }
}