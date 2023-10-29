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

namespace Hexalith.Infrastructure.WebApis.Parties.Helpers;

using Hexalith.Application.Events;
using Hexalith.Domain.Events;
using Hexalith.Infrastructure.WebApis.Parties.Controllers;
using Hexalith.Infrastructure.WebApis.Parties.IntegrationEvents;

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
    public static IServiceCollection AddPartiesIntegrationEventHandlers(this IServiceCollection services)
    {
        services.TryAddSingleton<IIntegrationEventHandler<CustomerRegistered>, CustomerRegisteredHandler>();
        services.TryAddSingleton<IIntegrationEventHandler<CustomerInformationChanged>, CustomerInformationChangedHandler>();
        _ = services
         .AddControllers()
         .AddApplicationPart(typeof(PartiesIntegrationEventsController).Assembly)
         .AddDapr();
        return services;
    }
}