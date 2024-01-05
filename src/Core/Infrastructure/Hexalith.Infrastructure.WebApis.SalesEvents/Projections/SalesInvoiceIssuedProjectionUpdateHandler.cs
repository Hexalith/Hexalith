// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.WebApis.SalesEvents
// Author           : Jérôme Piquot
// Created          : 11-12-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 11-12-2023
// ***********************************************************************
// <copyright file="SalesInvoiceRegisteredProjectionUpdateHandler.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.WebApis.SalesEvents.Projections;

using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Events;
using Hexalith.Infrastructure.DaprRuntime.Projections;

using Microsoft.Extensions.Logging;

/// <summary>
/// Class SalesInvoiceRegisteredProjectionUpdateHandler.
/// Implements the <see cref="Application.Events.IntegrationEventProjectionUpdateHandler{SalesInvoiceRegistered}" />.
/// </summary>
/// <seealso cref="Application.Events.IntegrationEventProjectionUpdateHandler{SalesInvoiceRegistered}" />
public class SalesInvoiceIssuedProjectionUpdateHandler : SalesInvoiceProjectionUpdateHandler<SalesInvoiceIssued>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SalesInvoiceRegisteredProjectionUpdateHandler"/> class.
    /// </summary>
    /// <param name="factory">The factory.</param>
    /// <param name="logger">The logger.</param>
    public SalesInvoiceIssuedProjectionUpdateHandler(IActorProjectionFactory<SalesInvoiceState> factory)
        : base(factory)
    {
    }
}