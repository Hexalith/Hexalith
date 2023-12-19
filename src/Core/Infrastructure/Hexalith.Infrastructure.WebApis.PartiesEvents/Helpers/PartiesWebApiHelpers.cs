// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.WebApis.Parties
// Author           : Jérôme Piquot
// Created          : 10-27-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-27-2023
// ***********************************************************************
// <copyright file="PartiesWebApiHelpers.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.WebApis.PartiesEvents.Helpers;

using Dapr.Actors.Client;

using Hexalith.Application.Parties.Services;
using Hexalith.Application.Projection;
using Hexalith.Domain.Events;
using Hexalith.Infrastructure.WebApis.PartiesEvents.Controllers;
using Hexalith.Infrastructure.WebApis.PartiesEvents.Projections;
using Hexalith.Infrastructure.WebApis.PartiesEvents.Services;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// Class PartiesWebApiHelpers.
/// </summary>
public static class PartiesWebApiHelpers
{
    /// <summary>
    /// Adds the customer projections.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="appName">Name of the application.</param>
    /// <returns>IServiceCollection.</returns>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public static IServiceCollection AddCustomerProjections(this IServiceCollection services, string appName)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentException.ThrowIfNullOrEmpty(appName);
        services.TryAddSingleton<IProjectionUpdateHandler<CustomerInformationChanged>, CustomerInformationChangedProjectionUpdateHandler>();
        services.TryAddSingleton<IProjectionUpdateHandler<CustomerRegistered>, CustomerRegisteredProjectionUpdateHandler>();
        services.TryAddSingleton<IProjectionUpdateHandler<IntercompanyDropshipDeliveryForCustomerDeselected>, IntercompanyDropshipDeliveryForCustomerDeselectedProjectionUpdateHandler>();
        services.TryAddSingleton<IProjectionUpdateHandler<IntercompanyDropshipDeliveryForCustomerSelected>, IntercompanyDropshipDeliveryForCustomerSelectedProjectionUpdateHandler>();
        services.TryAddSingleton<ICustomerProjectionService, CustomerAggregateQueryService>();
        services.TryAddSingleton<ICustomerProjectionActorFactory>(s
            => new CustomerProjectionActorFactory(s.GetRequiredService<IActorProxyFactory>(), appName));
        _ = services
         .AddControllers()
         .AddApplicationPart(typeof(CustomerIntegrationEventsController).Assembly)
         .AddDapr();
        return services;
    }
}