// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Dynamics365Finance.Inventories
// Author           : Jérôme Piquot
// Created          : 10-09-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-31-2023
// ***********************************************************************
// <copyright file="Dynamics365FinanceInventoriesHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Infrastructure.Dynamics365Finance.Inventories.Helpers;

using System.Diagnostics.CodeAnalysis;

using Dapr.Actors.Runtime;

using Hexalith.Extensions.Configuration;
using Hexalith.Infrastructure.Dynamics365Finance.Helpers;
using Hexalith.Infrastructure.Dynamics365Finance.Inventories.Configuration;
using Hexalith.Infrastructure.Dynamics365Finance.Inventories.Inventory;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Class InventoriesHelper.
/// </summary>
public static class Dynamics365FinanceInventoriesHelper
{
    /// <summary>
    /// Adds the dynamics365 finance customers.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <param name="applicationName">Name of the application.</param>
    /// <returns>IServiceCollection.</returns>
    /// <exception cref="ArgumentNullException">null.</exception>
    public static IServiceCollection AddDynamics365FinanceProductBarcodes(this IServiceCollection services, IConfiguration configuration, string applicationName)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentException.ThrowIfNullOrWhiteSpace(applicationName);
        _ = services.ConfigureSettings<Dynamics365FinanceInventoriesSettings>(configuration);
        return services
            .AddDynamics365FinanceProductBarcodesClient(configuration)
            .AddDynamics365FinanceProductBarcodesBusinessEvents(configuration, applicationName);
    }

    /// <summary>
    /// Adds the dynamics365 finance customers business events.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <param name="applicationName">Name of the application.</param>
    /// <returns>IServiceCollection.</returns>
    /// <exception cref="ArgumentNullException">null.</exception>
    public static IServiceCollection AddDynamics365FinanceProductBarcodesBusinessEvents(this IServiceCollection services, IConfiguration configuration, string applicationName)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentException.ThrowIfNullOrWhiteSpace(applicationName);
        _ = services
            .ConfigureSettings<Dynamics365FinanceInventoriesSettings>(configuration);
        _ = services
            .AddControllers()

            // .AddApplicationPart(typeof(Dynamics365FinanceProductBarcodeBindingController).Assembly)
            .AddDapr();
        return services;
    }

    /// <summary>
    /// Adds the dynamics365 finance customers client.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>IServiceCollection.</returns>
    /// <exception cref="ArgumentNullException">null.</exception>
    public static IServiceCollection AddDynamics365FinanceProductBarcodesClient(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        _ = services.AddDynamics365FinanceClient<ProductBarcode>(configuration);

        return services;
    }

    /// <summary>
    /// Adds the dynamics365 finance projections.
    /// </summary>
    /// <param name="actors">The actors.</param>
    /// <param name="applicationName">Name of the application.</param>
    /// <returns>ActorRegistrationCollection.</returns>
    /// <exception cref="ArgumentNullException">null.</exception>
#pragma warning disable IDE0060 // Remove unused parameter

    public static ActorRegistrationCollection AddDynamics365FinanceProjections([NotNull] this ActorRegistrationCollection actors, string applicationName)
#pragma warning restore IDE0060 // Remove unused parameter
    {
        ArgumentNullException.ThrowIfNull(actors);
        return actors;
    }
}