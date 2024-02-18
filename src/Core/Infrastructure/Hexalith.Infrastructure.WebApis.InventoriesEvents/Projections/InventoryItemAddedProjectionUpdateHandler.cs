// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.WebApis.PartiesEvents
// Author           : Jérôme Piquot
// Created          : 11-12-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 11-12-2023
// ***********************************************************************
// <copyright file="InventoryItemAddedProjectionUpdateHandler.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.WebApis.InventoriesEvents.Projections;

using Hexalith.Application.Inventories.InventoryItems.Projections;
using Hexalith.Domain.InventoryItems.Events;
using Hexalith.Infrastructure.DaprRuntime.Projections;
using Hexalith.Infrastructure.WebApis.PartiesEvents.Projections;

using Microsoft.Extensions.Logging;

/// <summary>
/// Class InventoryItemAddedProjectionUpdateHandler.
/// Implements the <see cref="Application.Events.IntegrationEventProjectionUpdateHandler{InventoryItemAdded}" />.
/// </summary>
/// <seealso cref="Application.Events.IntegrationEventProjectionUpdateHandler{InventoryItemAdded}" />
public class InventoryItemAddedProjectionUpdateHandler : InventoryItemDetailsProjectionUpdateHandler<InventoryItemAdded>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryItemAddedProjectionUpdateHandler"/> class.
    /// </summary>
    /// <param name="factory">The factory.</param>
    /// <param name="logger">The logger.</param>
    public InventoryItemAddedProjectionUpdateHandler(IActorProjectionFactory<InventoryItemDetailsProjection> factory, ILogger<InventoryItemAddedProjectionUpdateHandler> logger)
        : base(factory, logger)
    {
    }
}