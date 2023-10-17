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
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.BusinessEvents;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Entities;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Class PartiesHelper.
/// </summary>
public static class PartiesHelper
{
    /// <summary>
    /// Adds the fin ops customers.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns>IServiceCollection.</returns>
    public static IServiceCollection AddDynamics365FinanceCustomers(this IServiceCollection services)
    {
        return services
            .AddSingleton<IDynamics365FinanceClient<CustomerExternalSystemCode>, Dynamics365FinanceClient<CustomerExternalSystemCode>>()
            .AddSingleton<IDynamics365FinanceClient<CustomerV3>, Dynamics365FinanceClient<CustomerV3>>()
            .AddSingleton<IValidator<Dynamics365FinanceCustomerChanged>, Dynamics365FinanceCustomerChangedValidator>()
            .AddSingleton<IValidator<Dynamics365FinanceCustomerRegistered>, Dynamics365FinanceCustomerRegisteredValidator>()
            .AddSingleton<IntegrationEventHandler<Dynamics365FinanceCustomerChanged>, Dynamics365FinanceCustomerChangedHandler>()
            .AddSingleton<IntegrationEventHandler<Dynamics365FinanceCustomerRegistered>, Dynamics365FinanceCustomerRegisteredHandler>();
    }
}