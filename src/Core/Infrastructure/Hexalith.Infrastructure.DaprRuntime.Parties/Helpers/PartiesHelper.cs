// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprRuntime.Customer
// Author           : Jérôme Piquot
// Created          : 09-12-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 09-12-2023
// ***********************************************************************
// <copyright file="PartiesHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.DaprRuntime.Parties.Helpers;

using Hexalith.Application.Parties.Services;
using Hexalith.Extensions.Configuration;
using Hexalith.Infrastructure.DaprRuntime.Parties.Configurations;
using Hexalith.Infrastructure.DaprRuntime.Parties.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Class PartiesHelper.
/// </summary>
public static class PartiesHelper
{
    /// <summary>
    /// Adds the parties.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>IServiceCollection.</returns>
    public static IServiceCollection AddParties(this IServiceCollection services, IConfiguration configuration)
        => services
            .ConfigureSettings<CustomerSettings>(configuration)
            .AddTransient<ICustomerQueryService, CustomerQueryService>();
}