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

namespace Hexalith.Infrastructure.WebApis.PartiesCommands.Helpers;

using System.Diagnostics.CodeAnalysis;

using Dapr.Actors.Client;

using Hexalith.Application.Aggregates;
using Hexalith.Application.Commands;
using Hexalith.Application.Parties.Helpers;
using Hexalith.Domain.Aggregates;
using Hexalith.Infrastructure.DaprRuntime.CosmosDatabases.Maintenances;
using Hexalith.Infrastructure.DaprRuntime.Handlers;
using Hexalith.Infrastructure.WebApis.PartiesCommands.Controllers;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

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
    public static IServiceCollection AddPartiesCommandsSubmission([NotNull] this IServiceCollection services)
    {
        services
            .AddPartiesCommandHandlers()
            .TryAddSingleton<ICommandProcessor>((s) => new AggregateActorCommandProcessor(
                ActorProxy.DefaultProxyFactory,
                s.GetRequiredService<ILogger<AggregateActorCommandProcessor>>()));

        services.TryAddSingleton<IAggregateFactory, AggregateFactory>();
        services.TryAddSingleton<IAggregateProvider, AggregateProvider<Customer>>();
        services.TryAddTransient<IAggregateMaintenance<Customer>, DaprCosmosAggregateMaintenance<Customer>>();
        _ = services
         .AddControllers()
         .AddApplicationPart(typeof(PartiesCommandsController).Assembly)
         .AddDapr();
        return services;
    }
}