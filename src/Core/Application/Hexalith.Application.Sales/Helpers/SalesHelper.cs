// ***********************************************************************
// Assembly         : Hexalith.Application.Sales
// Author           : Jérôme Piquot
// Created          : 09-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 09-04-2023
// ***********************************************************************
// <copyright file="SalesHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Sales.Helpers;

using FluentValidation;

using Hexalith.Application.Commands;
using Hexalith.Application.Sales.CommandHandlers;
using Hexalith.Application.Sales.Commands;
using Hexalith.Domain.Events;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// Class SalesHelper.
/// </summary>
public static class SalesHelper
{
    /// <summary>
    /// Adds the parties command handlers.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns>IServiceCollection.</returns>
    public static IServiceCollection AddSalesCommandHandlers(this IServiceCollection services)
    {
        services.TryAddScoped<ICommandHandler<IssueSalesInvoice>, IssueSalesInvoiceHandler>();
        return services.AddSalesEventValidators();
    }

    /// <summary>
    /// Adds the parties event validators.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns>IServiceCollection.</returns>
    public static IServiceCollection AddSalesEventValidators(this IServiceCollection services)
    {
        services.TryAddSingleton<IValidator<SalesInvoiceIssued>, SalesInvoiceIssuedValidator>();
        return services;
    }
}