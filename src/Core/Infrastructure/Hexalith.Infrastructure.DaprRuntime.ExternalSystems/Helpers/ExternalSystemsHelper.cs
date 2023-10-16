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

using Dapr.Actors.Runtime;

using Hexalith.Application.ExternalSystems.Helpers;
using Hexalith.Application.ExternalSystems.Services;
using Hexalith.Extensions.Configuration;
using Hexalith.Infrastructure.DaprRuntime.ExternalSystems.Actors;
using Hexalith.Infrastructure.DaprRuntime.ExternalSystems.Configurations;
using Hexalith.Infrastructure.DaprRuntime.ExternalSystems.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
    public static IServiceCollection AddDaprExternalSystems(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddExternalSystemsCommandHandlers()
            .ConfigureSettings<ExternalSystemReferenceSettings>(configuration)
            .ConfigureSettings<AggregateExternalReferenceSettings>(configuration)
            .AddTransient<IExternalSystemReferenceQueryService, ExternalSystemReferenceQueryService>()
            .AddTransient<IAggregateExternalReferenceQueryService, AggregateExternalReferenceQueryService>();

    /// <summary>
    /// Adds the external systems.
    /// </summary>
    /// <param name="actors">The actors.</param>
    /// <returns>ActorRegistrationCollection.</returns>
    public static ActorRegistrationCollection AddExternalSystems([NotNull] this ActorRegistrationCollection actors)
    {
        ArgumentNullException.ThrowIfNull(actors);
        actors.RegisterActor<ExternalSystemReferenceAggregateActor>();
        actors.RegisterActor<AggregateExternalReferenceAggregateActor>();
        return actors;
    }
}