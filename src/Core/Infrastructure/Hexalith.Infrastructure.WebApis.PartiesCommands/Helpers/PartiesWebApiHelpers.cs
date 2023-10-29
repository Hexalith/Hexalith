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

using Hexalith.Application.Commands;
using Hexalith.Application.Parties.CommandHandlers;
using Hexalith.Application.Parties.Commands;
using Hexalith.Infrastructure.DaprRuntime.Handlers;
using Hexalith.Infrastructure.WebApis.PartiesCommands.Controllers;

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
    public static IServiceCollection AddPartiesCommands([NotNull] this IServiceCollection services)
    {
        services.TryAddSingleton<ICommandHandler<ChangeCustomerInformation>, ChangeCustomerInformationHandler>();
        services.TryAddSingleton<ICommandHandler<RegisterCustomer>, RegisterCustomerHandler>();
        services.TryAddSingleton<ICommandHandler<RegisterOrChangeCustomer>, RegisterOrChangeCustomerHandler>();
        services.TryAddSingleton<ICommandHandler<SetIntercompanyDropshipDeliveryForCustomer>, SetIntercompanyDropshipDeliveryForCustomerHandler>();
        services.TryAddSingleton<ICommandHandler<SetStockDeliveryForCustomer>, SetStockDeliveryForCustomerHandler>();
        services.TryAddSingleton(new ConventionNamingCommandProcessor(ActorProxy.DefaultProxyFactory));

        _ = services
         .AddControllers()
         .AddApplicationPart(typeof(PartiesCommandsController).Assembly)
         .AddDapr();
        return services;
    }
}