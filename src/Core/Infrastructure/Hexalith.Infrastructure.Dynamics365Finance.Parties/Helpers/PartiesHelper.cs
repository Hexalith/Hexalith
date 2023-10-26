// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Dynamics365Finance.Parties
// Author           : Jérôme Piquot
// Created          : 10-09-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-09-2023
// ***********************************************************************
// <copyright file="PartiesHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Infrastructure.Dynamics365Finance.Parties.Helpers;

using FluentValidation;

using Hexalith.Application.Events;
using Hexalith.Infrastructure.Dynamics365Finance.Client;
using Hexalith.Infrastructure.Dynamics365Finance.Helpers;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.BusinessEvents;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Entities;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// Class PartiesHelper.
/// </summary>
public static class PartiesHelper
{
    /// <summary>
    /// Adds the dynamics365 finance customers.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>IServiceCollection.</returns>
    public static IServiceCollection AddDynamics365FinanceCustomers(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDynamics365FinanceBusinessEvents(configuration)
            .AddDynamics365FinanceClient(configuration)
            .TryAddSingleton<IDynamics365FinanceClient<CustomerExternalSystemCode>, Dynamics365FinanceClient<CustomerExternalSystemCode>>();
        services.TryAddSingleton<IDynamics365FinanceClient<CustomerV3>, Dynamics365FinanceClient<CustomerV3>>();
        services.TryAddSingleton<IValidator<Dynamics365FinanceCustomerChanged>, Dynamics365FinanceCustomerChangedValidator>();
        services.TryAddSingleton<IValidator<Dynamics365FinanceCustomerRegistered>, Dynamics365FinanceCustomerRegisteredValidator>();
        services.TryAddSingleton<IIntegrationEventHandler<Dynamics365FinanceCustomerChanged>, Dynamics365FinanceCustomerChangedHandler>();
        services.TryAddSingleton<IIntegrationEventHandler<Dynamics365FinanceCustomerRegistered>, Dynamics365FinanceCustomerRegisteredHandler>();
        return services;
    }
}