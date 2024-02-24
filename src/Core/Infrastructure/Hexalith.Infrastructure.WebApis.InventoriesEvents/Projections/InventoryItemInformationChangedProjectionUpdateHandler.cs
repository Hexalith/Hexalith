// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.WebApis.PartiesEvents
// Author           : Jérôme Piquot
// Created          : 11-12-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 11-12-2023
// ***********************************************************************
// <copyright file="InventoryItemInformationChangedProjectionUpdateHandler.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.WebApis.InventoriesEvents.Projections;

using Hexalith.Application.Events;
using Hexalith.Application.Inventories.InventoryItems.Projections;
using Hexalith.Domain.InventoryItems.Events;
using Hexalith.Infrastructure.DaprRuntime.Projections;
using Hexalith.Infrastructure.WebApis.PartiesEvents.Projections;

using Microsoft.Extensions.Logging;

/// <summary>
/// Class InventoryItemInformationChangedHandler.
/// Implements the <see cref="IntegrationEventHandlerBase{InventoryItemInformationChanged}" />.
/// </summary>
/// <seealso cref="IntegrationEventHandlerBase{InventoryItemInformationChanged}" />
public class InventoryItemInformationChangedProjectionUpdateHandler : InventoryItemDetailsProjectionUpdateHandler<InventoryItemInformationChanged>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryItemInformationChangedProjectionUpdateHandler"/> class.
    /// </summary>
    /// <param name="factory">The factory.</param>
    /// <param name="logger">The logger.</param>
    public InventoryItemInformationChangedProjectionUpdateHandler(IActorProjectionFactory<InventoryItemDetailsProjection> factory, ILogger<InventoryItemInformationChangedProjectionUpdateHandler> logger)
        : base(factory, logger)
    {
    }
}