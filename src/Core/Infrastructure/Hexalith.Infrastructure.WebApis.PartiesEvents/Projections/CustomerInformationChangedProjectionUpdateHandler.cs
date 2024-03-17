// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.WebApis.PartiesEvents
// Author           : Jérôme Piquot
// Created          : 11-12-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 11-12-2023
// ***********************************************************************
// <copyright file="CustomerInformationChangedProjectionUpdateHandler.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.WebApis.PartiesEvents.Projections;

using Hexalith.Application.Events;
using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Events;
using Hexalith.Infrastructure.DaprRuntime.Projections;

using Microsoft.Extensions.Logging;

/// <summary>
/// Class CustomerInformationChangedHandler.
/// Implements the <see cref="IntegrationEventHandlerBase{CustomerInformationChanged}" />.
/// </summary>
/// <seealso cref="IntegrationEventHandlerBase{CustomerInformationChanged}" />
/// <remarks>
/// Initializes a new instance of the <see cref="CustomerInformationChangedProjectionUpdateHandler"/> class.
/// </remarks>
/// <param name="factory">The factory.</param>
/// <param name="logger">The logger.</param>
public class CustomerInformationChangedProjectionUpdateHandler(IActorProjectionFactory<Customer> factory, ILogger<CustomerInformationChangedProjectionUpdateHandler> logger)
    : CustomerProjectionUpdateHandler<CustomerInformationChanged>(factory, logger)
{
}