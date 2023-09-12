// ***********************************************************************
// Assembly         : Hexalith.Application.Parties
// Author           : Jérôme Piquot
// Created          : 09-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 09-04-2023
// ***********************************************************************
// <copyright file="InventoriesHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Inventories.Helpers;

using Hexalith.Application.Commands;
using Hexalith.Application.Inventories.CommandHandlers;
using Hexalith.Application.Inventories.Commands;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Class PartiesHelper.
/// </summary>
public static class InventoriesHelper
{
    /// <summary>
    /// Adds the parties command handlers.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns>IServiceCollection.</returns>
    public static IServiceCollection AddInventoriesCommandHandlers(this IServiceCollection services)
    {
        return services
            .AddTransient<ICommandHandler<AddInventoryItemBarcode>, AddInventoryItemBarcodeHandler>()
            .AddTransient<ICommandHandler<AddInventoryItem>, AddInventoryItemHandler>()
            .AddTransient<ICommandHandler<ChangeInventoryItemInformation>, ChangeInventoryItemInformationHandler>()
            .AddTransient<ICommandHandler<DecreaseInventoryItemStock>, DecreaseInventoryItemStockHandler>()
            .AddTransient<ICommandHandler<IncreaseInventoryItemStock>, IncreaseInventoryItemStockHandler>()
            .AddTransient<ICommandHandler<RemoveInventoryItemBarcode>, RemoveInventoryItemBarcodeHandler>();
    }
}