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
    /// Adds the external systems integration event handlers.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns>IServiceCollection.</returns>
    public static IServiceCollection AddCustomerAggregateProjection(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);
        services.TryAddSingleton<IProjectionUpdateHandler<CustomerInformationChanged>, CustomerInformationChangedProjectionUpdateHandler>();
        services.TryAddSingleton<IProjectionUpdateHandler<CustomerRegistered>, CustomerRegisteredProjectionUpdateHandler>();
        services.TryAddSingleton<IProjectionUpdateHandler<IntercompanyDropshipDeliveryForCustomerDeselected>, IntercompanyDropshipDeliveryForCustomerDeselectedProjectionUpdateHandler>();
        services.TryAddSingleton<IProjectionUpdateHandler<IntercompanyDropshipDeliveryForCustomerSelected>, IntercompanyDropshipDeliveryForCustomerSelectedProjectionUpdateHandler>();
        services.TryAddSingleton<ICustomerAggregateQueryService, CustomerAggregateQueryService>();
        _ = services
         .AddControllers()
         .AddApplicationPart(typeof(CustomerIntegrationEventsController).Assembly)
         .AddDapr();
        return services;
    }
}