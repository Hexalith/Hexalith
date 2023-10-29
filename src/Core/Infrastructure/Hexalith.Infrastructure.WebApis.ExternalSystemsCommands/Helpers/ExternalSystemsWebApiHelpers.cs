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

namespace Hexalith.Infrastructure.WebApis.ExternalSystemsCommands.Helpers;

using System.Diagnostics.CodeAnalysis;

using Dapr.Actors.Client;

using Hexalith.Application.Commands;
using Hexalith.Application.ExternalSystems.CommandHandlers;
using Hexalith.Application.ExternalSystems.Commands;
using Hexalith.Infrastructure.DaprRuntime.Handlers;
using Hexalith.Infrastructure.WebApis.ExternalSystemsCommands.Controllers;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// Class ExternalSystemsWebApiHelpers.
/// </summary>
public static class ExternalSystemsWebApiHelpers
{
    /// <summary>
    /// Adds the external systems commands.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns>IServiceCollection.</returns>
    public static IServiceCollection AddExternalSystemsCommands([NotNull] this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);
        services.TryAddSingleton<ICommandHandler<AddExternalSystemReference>, AddExternalSystemReferenceHandler>();
        services.TryAddSingleton<ICommandHandler<RemoveExternalSystemReference>, RemoveExternalSystemReferenceHandler>();
        services.TryAddSingleton(new ConventionNamingCommandProcessor(ActorProxy.DefaultProxyFactory));

        _ = services
         .AddControllers()
         .AddApplicationPart(typeof(ExternalSystemsIntegrationCommandsController).Assembly)
         .AddDapr();
        return services;
    }
}