// ***********************************************************************
// Assembly         : Hexalith.Application.Parties
// Author           : Jérôme Piquot
// Created          : 09-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 09-04-2023
// ***********************************************************************
// <copyright file="PartiesHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Parties.Helpers;

using Hexalith.Application.Commands;
using Hexalith.Application.Parties.CommandHandlers;
using Hexalith.Application.Parties.Commands;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// Class PartiesHelper.
/// </summary>
public static class PartiesHelper
{
    /// <summary>
    /// Adds the parties command handlers.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns>IServiceCollection.</returns>
    public static IServiceCollection AddPartiesCommandHandlers(this IServiceCollection services)
    {
        services.TryAddSingleton<ICommandHandler<ChangeCustomerInformation>, ChangeCustomerInformationHandler>();
        services.TryAddSingleton<ICommandHandler<RegisterCustomer>, RegisterCustomerHandler>();
        services.TryAddSingleton<ICommandHandler<RegisterOrChangeCustomer>, RegisterOrChangeCustomerHandler>();
        services.TryAddSingleton<ICommandHandler<SelectIntercompanyDropshipDeliveryForCustomer>, SelectIntercompanyDropshipDeliveryForCustomerHandler>();
        services.TryAddSingleton<ICommandHandler<DeselectIntercompanyDropshipDeliveryForCustomer>, DeselectIntercompanyDropshipDeliveryForCustomerHandler>();
        return services;
    }
}