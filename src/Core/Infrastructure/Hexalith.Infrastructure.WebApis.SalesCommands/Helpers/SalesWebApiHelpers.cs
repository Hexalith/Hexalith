// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.WebApis.Sales
// Author           : Jérôme Piquot
// Created          : 10-27-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-27-2023
// ***********************************************************************
// <copyright file="SalesWebApiHelpers.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.WebApis.SalesCommands.Helpers;

using System.Diagnostics.CodeAnalysis;

using Dapr.Actors.Client;

using Hexalith.Application.Aggregates;
using Hexalith.Application.Commands;
using Hexalith.Application.Sales.Helpers;
using Hexalith.Domain.Aggregates;
using Hexalith.Infrastructure.DaprRuntime.Handlers;
using Hexalith.Infrastructure.WebApis.SalesCommands.Controllers;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

/// <summary>
/// Class SalesWebApiHelpers.
/// </summary>
public static class SalesWebApiHelpers
{
    /// <summary>
    /// Adds the external systems integration event handlers.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns>IServiceCollection.</returns>
    public static IServiceCollection AddSalesCommandsSubmission([NotNull] this IServiceCollection services)
    {
        services
            .AddSalesCommandHandlers()
            .TryAddSingleton<ICommandProcessor>((s) => new AggregateActorCommandProcessor(
                ActorProxy.DefaultProxyFactory,
                s.GetRequiredService<ILogger<AggregateActorCommandProcessor>>()));

        services.TryAddSingleton<IAggregateFactory, AggregateFactory>();
        services.TryAddSingleton<IAggregateProvider, AggregateProvider<SalesInvoice>>();
        _ = services
         .AddControllers()
         .AddApplicationPart(typeof(SalesCommandsController).Assembly)
         .AddDapr();
        return services;
    }
}