// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprRuntime.Customer
// Author           : Jérôme Piquot
// Created          : 09-12-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-15-2023
// ***********************************************************************
// <copyright file="SalesHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.DaprRuntime.Sales.Helpers;

using System.Diagnostics.CodeAnalysis;

using Hexalith.Application.Sales.Helpers;
using Hexalith.Extensions.Configuration;
using Hexalith.Infrastructure.DaprRuntime.Sales.Configurations;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Class SalesHelper.
/// </summary>
public static class SalesHelper
{
    /// <summary>
    /// Adds the parties.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>IServiceCollection.</returns>
    public static IServiceCollection AddDaprSales([NotNull] this IServiceCollection services, [NotNull] IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);
        _ = services
            .AddSalesCommandHandlers()
            .ConfigureSettings<SalesInvoiceSettings>(configuration);
        return services;
    }

    /// <summary>
    /// Adds the dapr parties client.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns>IServiceCollection.</returns>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public static IServiceCollection AddDaprSalesClient([NotNull] this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        // services.TryAddScoped<ICustomerQueryService, ActorCustomerQueryService>();
        return services;
    }
}