// ***********************************************************************
// Assembly         : Bspk.InventoryOnHands
// Author           : Jérôme Piquot
// Created          : 08-30-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-30-2023
// ***********************************************************************
// <copyright file="IInventoryItemStockAggregateActor.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.DaprRuntime.Inventories.Actors;

using System.Threading.Tasks;

using Dapr.Actors;

/// <summary>
/// Interface IInventoryOnHandAggregateActor
/// Extends the <see cref="IActor" />.
/// </summary>
/// <seealso cref="IActor" />
public interface IInventoryItemStockAggregateActor : IActor
{
    /// <summary>
    /// Checks if the InventoryOnHand exists.
    /// </summary>
    /// <returns>Task&lt;System.Boolean&gt;.</returns>
    Task<bool> ExistAsync();

    /// <summary>
    /// Stock level of an inventory item.
    /// </summary>
    /// <returns>Task&lt;System.Decimal&gt;.</returns>
    Task<decimal> LevelAsync();
}