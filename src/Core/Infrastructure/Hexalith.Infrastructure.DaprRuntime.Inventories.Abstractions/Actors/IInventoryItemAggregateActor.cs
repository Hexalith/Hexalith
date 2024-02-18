// ***********************************************************************
// Assembly         : Bspk.InventoryOnHands
// Author           : Jérôme Piquot
// Created          : 08-30-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-30-2023
// ***********************************************************************
// <copyright file="IInventoryItemAggregateActor.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.DaprRuntime.Inventories.Actors;

using System.Threading.Tasks;

using Dapr.Actors;

using Hexalith.Domain.InventoryItems.Events;

/// <summary>
/// Interface IInventoryOnHandAggregateActor
/// Extends the <see cref="IActor" />.
/// </summary>
/// <seealso cref="IActor" />
public interface IInventoryItemAggregateActor : IActor
{
    /// <summary>
    /// Creates the information changed event asynchronous.
    /// </summary>
    /// <returns>Task&lt;InventoryOnHandInformationChanged&gt;.</returns>
    Task<InventoryItemInformationChanged?> CreateInformationChangedEventAsync();

    /// <summary>
    /// Checks if the InventoryOnHand exists.
    /// </summary>
    /// <returns>Task&lt;System.Boolean&gt;.</returns>
    Task<bool> ExistAsync();

    /// <summary>
    /// Gets the item name asynchronous.
    /// </summary>
    /// <returns>Task&lt;System.Nullable&lt;System.String&gt;&gt;.</returns>
    Task<string?> GetItemNameAsync();

    /// <summary>
    /// Determines whether [has changes asynchronous] [the specified change].
    /// </summary>
    /// <param name="changed">The changed event.</param>
    /// <returns>Task&lt;System.Boolean&gt;.</returns>
    Task<bool> HasChangesAsync(InventoryItemInformationChanged changed);
}