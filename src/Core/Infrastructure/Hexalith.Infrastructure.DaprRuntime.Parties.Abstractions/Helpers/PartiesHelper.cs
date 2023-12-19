// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprRuntime.Customer
// Author           : Jérôme Piquot
// Created          : 09-12-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-15-2023
// ***********************************************************************
// <copyright file="PartiesHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.DaprRuntime.Parties.Helpers;

using System.Diagnostics.CodeAnalysis;

using Hexalith.Application.Parties.Helpers;
using Hexalith.Application.Parties.Services;
using Hexalith.Extensions.Configuration;
using Hexalith.Infrastructure.DaprRuntime.Parties.Configurations;
using Hexalith.Infrastructure.DaprRuntime.Parties.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

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
    public static IServiceCollection AddDaprParties([NotNull] this IServiceCollection services, [NotNull] IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);
        _ = services
            .AddPartiesCommandHandlers()
            .ConfigureSettings<CustomerSettings>(configuration);
        return services;
    }

    /// <summary>
    /// Adds the dapr parties client.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns>IServiceCollection.</returns>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public static IServiceCollection AddDaprPartiesClient([NotNull] this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);
        services.TryAddScoped<ICustomerQueryService, ActorCustomerQueryService>();
        return services;
    }
}