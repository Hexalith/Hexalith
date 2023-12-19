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

using FluentValidation;

using Hexalith.Application.Commands;
using Hexalith.Application.Parties.CommandHandlers;
using Hexalith.Application.Parties.Commands;
using Hexalith.Domain.Events;

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
        services.TryAddScoped<ICommandHandler<ChangeCustomerInformation>, ChangeCustomerInformationHandler>();
        services.TryAddScoped<ICommandHandler<RegisterCustomer>, RegisterCustomerHandler>();
        services.TryAddScoped<ICommandHandler<RegisterOrChangeCustomer>, RegisterOrChangeCustomerHandler>();
        services.TryAddScoped<ICommandHandler<SelectIntercompanyDropshipDeliveryForCustomer>, SelectIntercompanyDropshipDeliveryForCustomerHandler>();
        services.TryAddScoped<ICommandHandler<DeselectIntercompanyDropshipDeliveryForCustomer>, DeselectIntercompanyDropshipDeliveryForCustomerHandler>();
        return services.AddPartiesEventValidators();
    }

    /// <summary>
    /// Adds the parties event validators.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns>IServiceCollection.</returns>
    public static IServiceCollection AddPartiesEventValidators(this IServiceCollection services)
    {
        services.TryAddSingleton<IValidator<CustomerInformationChanged>, CustomerInformationChangedValidator>();
        services.TryAddSingleton<IValidator<CustomerRegistered>, CustomerRegisteredValidator>();
        services.TryAddSingleton<IValidator<IntercompanyDropshipDeliveryForCustomerDeselected>, IntercompanyDropshipDeliveryForCustomerDeselectedValidator>();
        services.TryAddSingleton<IValidator<IntercompanyDropshipDeliveryForCustomerSelected>, IntercompanyDropshipDeliveryForCustomerSelectedValidator>();
        return services;
    }
}