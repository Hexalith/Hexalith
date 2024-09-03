// ***********************************************************************
// Assembly         : Hexalith.Application.Organizations
// Author           : Jérôme Piquot
// Created          : 10-16-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-16-2023
// ***********************************************************************
// <copyright file="OrganizationHelper.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Application.Organizations.Helpers;

using Hexalith.Application.Organizations.Configurations;
using Hexalith.Extensions.Configuration;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Class OrganizationHelper.
/// </summary>
public static class OrganizationHelper
{
    /// <summary>
    /// Adds the organizations.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>IServiceCollection.</returns>
    public static IServiceCollection AddOrganizations(this IServiceCollection services, IConfiguration configuration)
    => services
        .ConfigureSettings<OrganizationSettings>(configuration);
}