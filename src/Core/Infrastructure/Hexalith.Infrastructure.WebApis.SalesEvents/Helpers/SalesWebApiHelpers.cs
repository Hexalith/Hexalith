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

namespace Hexalith.Infrastructure.WebApis.SalesEvents.Helpers;

using Hexalith.Application.Projections;
using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Events;
using Hexalith.Infrastructure.DaprRuntime.Helpers;
using Hexalith.Infrastructure.WebApis.SalesEvents.Controllers;
using Hexalith.Infrastructure.WebApis.SalesEvents.Projections;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// Class SalesWebApiHelpers.
/// </summary>
public static class SalesWebApiHelpers
{
    /// <summary>
    /// Adds the customer projections.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="appName">Name of the application.</param>
    /// <returns>IServiceCollection.</returns>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public static IServiceCollection AddSalesInvoiceProjections(this IServiceCollection services, string appName)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentException.ThrowIfNullOrWhiteSpace(appName);
        services.TryAddScoped<IProjectionUpdateHandler<SalesInvoiceIssued>, SalesInvoiceIssuedProjectionUpdateHandler>();
        _ = services.AddActorProjectionFactory<SalesInvoiceState>(appName);
        _ = services
         .AddControllers()
         .AddApplicationPart(typeof(SalesInvoiceIntegrationEventsController).Assembly)
         .AddDapr();
        return services;
    }
}