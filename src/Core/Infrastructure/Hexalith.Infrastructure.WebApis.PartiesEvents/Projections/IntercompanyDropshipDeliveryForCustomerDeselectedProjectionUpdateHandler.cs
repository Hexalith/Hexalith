// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.WebApis.PartiesEvents
// Author           : Jérôme Piquot
// Created          : 11-12-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 11-12-2023
// ***********************************************************************
// <copyright file="IntercompanyDropshipDeliveryForCustomerDeselectedProjectionUpdateHandler.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.WebApis.PartiesEvents.Projections;

using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Events;
using Hexalith.Infrastructure.DaprRuntime.Projections;

using Microsoft.Extensions.Logging;

/// <summary>
/// Class IntercompanyDropshipDeliveryForCustomerDeselectedHandler.
/// Implements the <see cref="Application.Events.IntegrationEventProjectionUpdateHandler{CustomerRegistered}" />.
/// </summary>
/// <seealso cref="Application.Events.IntegrationEventProjectionUpdateHandler{CustomerRegistered}" />
public class IntercompanyDropshipDeliveryForCustomerDeselectedProjectionUpdateHandler : CustomerProjectionUpdateHandler<IntercompanyDropshipDeliveryForCustomerDeselected>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IntercompanyDropshipDeliveryForCustomerDeselectedProjectionUpdateHandler" /> class.
    /// </summary>
    /// <param name="stateStoreProvider">The state store provider.</param>
    public IntercompanyDropshipDeliveryForCustomerDeselectedProjectionUpdateHandler(IActorProjectionFactory<Customer> factory, ILogger<IntercompanyDropshipDeliveryForCustomerDeselectedProjectionUpdateHandler> logger)
        : base(factory, logger)
    {
    }
}