// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.WebApis.ExternalSystems
// Author           : Jérôme Piquot
// Created          : 10-27-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-27-2023
// ***********************************************************************
// <copyright file="ExternalSystemsWebApiHelpers.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.WebApis.ExternalSystems.Helpers;

using Hexalith.Application.Projection;
using Hexalith.Domain.Events;
using Hexalith.Infrastructure.DaprRuntime.ExternalSystems.Projections;
using Hexalith.Infrastructure.WebApis.ExternalSystems.Controllers;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// Class ExternalSystemsWebApiHelpers.
/// </summary>
public static class ExternalSystemsWebApiHelpers
{
    /// <summary>
    /// Adds the external systems mapper.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="applicationName">Name of the application.</param>
    /// <returns>IServiceCollection.</returns>
    public static IServiceCollection AddExternalSystemsMapperUpdate(this IServiceCollection services, string applicationName)
    {
        services.TryAddSingleton<IProjectionUpdateHandler<ExternalSystemReferenceAdded>>(new ExternalSystemReferenceAddedMapperUpdateHandler(applicationName));
        services.TryAddSingleton<IProjectionUpdateHandler<ExternalSystemReferenceRemoved>>(new ExternalSystemReferenceRemovedMapperUpdateHandler(applicationName));
        _ = services
         .AddControllers()
         .AddApplicationPart(typeof(ExternalSystemsIntegrationEventsController).Assembly)
         .AddDapr();
        return services;
    }
}