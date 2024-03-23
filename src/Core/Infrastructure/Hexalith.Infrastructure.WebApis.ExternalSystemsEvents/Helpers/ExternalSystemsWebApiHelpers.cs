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

namespace Hexalith.Infrastructure.WebApis.ExternalSystemsEvents.Helpers;

using System.Diagnostics.CodeAnalysis;

using Hexalith.Application.ExternalSystems.Services;
using Hexalith.Application.Projections;
using Hexalith.Domain.Events;
using Hexalith.Infrastructure.DaprRuntime.ExternalSystems.Projections;
using Hexalith.Infrastructure.DaprRuntime.ExternalSystems.Services;
using Hexalith.Infrastructure.WebApis.ExternalSystemsEvents.Controllers;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// Class ExternalSystemsWebApiHelpers.
/// </summary>
public static class ExternalSystemsWebApiHelpers
{
    /// <summary>
    /// Adds the external systems mapper subscription.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="applicationId">Name of the application.</param>
    /// <param name="aggregateNames">The aggregate names.</param>
    /// <returns>IServiceCollection.</returns>
    public static IServiceCollection AddExternalSystemsMapperSubscription(
        this IServiceCollection services,
        [NotNull] string applicationId,
        [NotNull] IEnumerable<string> aggregateNames)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentException.ThrowIfNullOrWhiteSpace(applicationId);
        ArgumentNullException.ThrowIfNull(aggregateNames);

        services.TryAddScoped<IProjectionUpdateHandler<ExternalSystemReferenceAdded>>(s => new ExternalSystemReferenceAddedMapperUpdateHandler(applicationId, aggregateNames));
        services.TryAddScoped<IProjectionUpdateHandler<ExternalSystemReferenceRemoved>>(s => new ExternalSystemReferenceRemovedMapperUpdateHandler(applicationId, aggregateNames));
        services.TryAddScoped<IExternalReferenceMapperService>(s => new ExternalReferenceMapperService(applicationId));
        _ = services
         .AddControllers()
         .AddApplicationPart(typeof(ExternalSystemsIntegrationEventsController).Assembly)
         .AddDapr();
        return services;
    }
}