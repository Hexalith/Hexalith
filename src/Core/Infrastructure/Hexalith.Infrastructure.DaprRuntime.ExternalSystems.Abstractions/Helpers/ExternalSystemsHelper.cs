// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprRuntime.ExternalSystems
// Author           : Jérôme Piquot
// Created          : 09-12-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 09-12-2023
// ***********************************************************************
// <copyright file="ExternalSystemsHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.DaprRuntime.ExternalSystems.Helpers;

using System.Diagnostics.CodeAnalysis;

using Hexalith.Application.ExternalSystems.Helpers;
using Hexalith.Application.ExternalSystems.Services;
using Hexalith.Extensions.Configuration;
using Hexalith.Infrastructure.DaprRuntime.ExternalSystems.Configurations;
using Hexalith.Infrastructure.DaprRuntime.ExternalSystems.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// Class ExternalSystemsHelper.
/// </summary>
public static class ExternalSystemsHelper
{
    /// <summary>
    /// Adds the external systems service.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>IServiceCollection.</returns>
    public static IServiceCollection AddDaprExternalSystems([NotNull] this IServiceCollection services, [NotNull] IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);
        services
            .AddExternalSystemsCommandHandlers()
            .ConfigureSettings<ExternalSystemReferenceSettings>(configuration)
            .ConfigureSettings<AggregateExternalReferenceSettings>(configuration)
            .TryAddTransient<IExternalSystemReferenceQueryService, ExternalSystemReferenceActorService>();
        services.TryAddTransient<IAggregateExternalReferenceQueryService, AggregateExternalReferenceQueryService>();
        return services;
    }

    /// <summary>
    /// Adds the dapr external systems client.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns>IServiceCollection.</returns>
    public static IServiceCollection AddDaprExternalSystemsClient([NotNull] this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);
        services.TryAddSingleton<IExternalSystemReferenceQueryService, ExternalSystemReferenceActorService>();
        services.TryAddSingleton<IAggregateExternalReferenceQueryService, AggregateExternalReferenceQueryService>();
        return services;
    }
}